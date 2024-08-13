using ISession = Net.Pkcs11Interop.HighLevelAPI.ISession;

namespace Pkcs11Test.CustomClasses
{
    public interface IHsmAdapter
    {
        void Initialize(string library, int slotNumber, string userPin);
        ISession OpenSession();
    }
}
