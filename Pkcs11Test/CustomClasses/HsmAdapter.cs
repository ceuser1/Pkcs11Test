using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Serilog;
using ISession = Net.Pkcs11Interop.HighLevelAPI.ISession;

namespace Pkcs11Test.CustomClasses
{
    public class HsmAdapter : IHsmAdapter, IDisposable
    {
        private bool _isInitialized;
        private IPkcs11Library _pkcs11;
        private ISlot _slot;
        private ISession _masterSession;

        private readonly object _lockInitialize = new ();
        public void Initialize(string library, int slotNumber, string userPin)
        {
            if (_isInitialized)
            {
                return;
            }

            lock (_lockInitialize)
            {
                Log.Warning("Initialize START");

                //// const string library = "/pkcs11/libcs_pkcs11_R2.so";
                //// const string library = "/pkcs11/pkcs11-logger-x64.so";
                //const string library = @"C:\src\_Temp\Pkcs11Test\Files\cs_pkcs11_R2.dll";
                //const int slotNumber = 0;
                //const string userPin = "12345678";

                // PKCS11.
                var factories = new Pkcs11InteropFactories();
                Log.Warning("LoadPkcs11Library START");
                _pkcs11 = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, library, AppType.MultiThreaded);
                Log.Warning("LoadPkcs11Library END");

                // Slot.
                Log.Warning("GetSlotList START");
                var slots = _pkcs11.GetSlotList(SlotsType.WithOrWithoutTokenPresent);
                Log.Warning("GetSlotList END");
                _slot = slots.FirstOrDefault(p => p.SlotId == (ulong)slotNumber);
                if (_slot == null)
                {
                    throw new Exception("Error initializing pkcs11 - unknown slot nr " + slotNumber);
                }

                // Login.
                Log.Warning("OpenSession START");
                _masterSession = _slot.OpenSession(SessionType.ReadWrite);
                Log.Warning("OpenSession END");
                Log.Warning("Login START");
                _masterSession.Login(CKU.CKU_USER, userPin);
                Log.Warning("Login END");

                _isInitialized = true;

                Log.Warning("Initialize END");
            }
        }

        public ISession OpenSession()
        {
            Log.Warning("OpenSession START");

            if (!_isInitialized)
            {
                throw new Exception("Error opening a new session - pkcs11 not initialized");
            }
            if (_masterSession == null)
            {
                throw new Exception("Error opening a new session - _masterSession is null");
            }
            if (_slot == null)
            {
                throw new Exception("Error opening a new session - _slot is null");
            }

            var session = _slot.OpenSession(SessionType.ReadWrite);
            Log.Warning("Successfully opened new session with id " + session.SessionId);

            Log.Warning("OpenSession END");
            return session;
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
                _masterSession?.Logout();
                _masterSession?.CloseSession();
                //Thread.Sleep(1000);
                _pkcs11?.Dispose();
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
