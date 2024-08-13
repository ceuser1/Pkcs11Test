using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Serilog;
using ISession = Net.Pkcs11Interop.HighLevelAPI.ISession;

namespace Pkcs11Test.CustomClasses
{
    public class Hsm : IDisposable
    {
        private readonly IHsmAdapter _adapter;
        private ISession _session;

        public Hsm(IHsmAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Initialize()
        {
            _session = _adapter.OpenSession();
        }

        public Tuple<ulong, ulong> GenerateKeyPair()
        {
            Log.Warning("GenerateKeyPair START");

            if (_session == null)
            {
                throw new Exception("Session is null");
            }

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
            Log.Warning("GenerateKeyPairImpl START");
            _session.GenerateKeyPair(
                _session.Factories.MechanismFactory.Create(mechanism),
                publicKeyAttributes,
                privateKeyAttributes,
                out var publicObjectHandle,
                out var privateObjectHandle
            );
            Log.Warning("GenerateKeyPairImpl END");

            Log.Warning("GenerateKeyPair END");
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
                _session?.CloseSession();
            }

            // Cleanup unmanaged objects.
            _isDisposed = true;
        }

        ~Hsm()
        {
            Dispose(false);
        }
        #endregion
    }
}
