using System.Security.Cryptography;
using System.Text;

namespace Bulgarikon.Data.Seed
{
    public static class DeterministicGuid
    {
        public static Guid Create(Guid @namespace, string name)
        {
            byte[] namespaceBytes = @namespace.ToByteArray();
            SwapGuidByteOrder(namespaceBytes);

            byte[] nameBytes = Encoding.UTF8.GetBytes(name);

            byte[] data = new byte[namespaceBytes.Length + nameBytes.Length];
            Buffer.BlockCopy(namespaceBytes, 0, data, 0, namespaceBytes.Length);
            Buffer.BlockCopy(nameBytes, 0, data, namespaceBytes.Length, nameBytes.Length);

            byte[] hash;
            using (var sha1 = SHA1.Create())
            {
                hash = sha1.ComputeHash(data);
            }

            byte[] newGuid = new byte[16];
            Array.Copy(hash, 0, newGuid, 0, 16);

            newGuid[6] = (byte)((newGuid[6] & 0x0F) | (5 << 4));

            newGuid[8] = (byte)((newGuid[8] & 0x3F) | 0x80);

            SwapGuidByteOrder(newGuid);
            return new Guid(newGuid);
        }

        private static void SwapGuidByteOrder(byte[] guid)
        {
            Swap(guid, 0, 3);
            Swap(guid, 1, 2);
            Swap(guid, 4, 5);
            Swap(guid, 6, 7);
        }

        private static void Swap(byte[] arr, int a, int b)
        {
            (arr[a], arr[b]) = (arr[b], arr[a]);
        }
    }
}