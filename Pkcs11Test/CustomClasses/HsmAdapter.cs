using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using ISession = Net.Pkcs11Interop.HighLevelAPI.ISession;

namespace Pkcs11Test.CustomClasses
{
    public class HsmAdapter : IDisposable
    {
        private IPkcs11Library _pkcs11;
        private ISession _session;

        public void Login()
        {
            const string library = "/pkcs11/libcs_pkcs11_R2.so";
            // const string library = "/pkcs11/pkcs11-logger-x64.so";
            const int slotNumber = 0;
            const string userPin = "12345678";

            // PKCS11.
            var factories = new Pkcs11InteropFactories();
            _pkcs11 = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, library, AppType.MultiThreaded);

            // Slot.
            var slots = _pkcs11.GetSlotList(SlotsType.WithOrWithoutTokenPresent);
            var slot = slots.FirstOrDefault(p => p.SlotId == slotNumber);
            if (slot == null)
            {
                throw new Exception("Error logging in");
            }

            // Login.
            var session = slot.OpenSession(SessionType.ReadWrite);
            session.Login(CKU.CKU_USER, userPin);
            _session = session;
        }

        public Tuple<ulong, ulong> GenerateKeyPair()
        {
            var name = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            // Key identifiers.
            var publicKeyName = name + " public";
            var publicKeyId = _session.GenerateRandom(20);
            var privateKeyName = name + " private";
            var privateKeyId = _session.GenerateRandom(20);

            // Key attributes.
            var publicKeyAttributes = new List<IObjectAttribute>();
            var privateKeyAttributes = new List<IObjectAttribute>();
            var mechanism = CKM.CKM_ECDSA_KEY_PAIR_GEN;

            // Public.
            publicKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ECDSA_PARAMS, new byte[] { 0x06, 0x08, 0x2A, 0x86, 0x48, 0xCE, 0x3D, 0x03, 0x01, 0x07 }));
            publicKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ID, publicKeyId));
            publicKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, publicKeyName));
            publicKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_PRIVATE, false));
            publicKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));
            publicKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_VERIFY, true));

            // Private.
            privateKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_ID, privateKeyId));
            privateKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_LABEL, privateKeyName));
            privateKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_PRIVATE, true));
            privateKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_SENSITIVE, false));
            privateKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_SIGN, true));
            privateKeyAttributes.Add(_session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, true));

            // Generate.
            _session.GenerateKeyPair(
                _session.Factories.MechanismFactory.Create(mechanism),
                publicKeyAttributes,
                privateKeyAttributes,
                out var publicObjectHandle,
                out var privateObjectHandle
            );

            return new Tuple<ulong, ulong>(publicObjectHandle.ObjectId, privateObjectHandle.ObjectId);
        }

        #region Dispose
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);

            // The GC.SuppressFinalize() method tells the Garbage Collector that a class no longer needs to have its destructor called.
            GC.SuppressFinalize(this);
        }

        // Called by both the destructor and by IDisposable.Dispose().
        // The parameter disposing indicates whether Dispose(bool) has been invoked by the destructor or by IDisposable.Dispose().
        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _session?.Logout();
                _session?.Dispose();
                _session = null;

                _pkcs11?.Dispose();
                _pkcs11 = null;
            }

            // Cleanup unmanaged objects.
            _isDisposed = true;
        }

        ~HsmAdapter()
        {
            Dispose(false);
        }
        #endregion
    }
}
