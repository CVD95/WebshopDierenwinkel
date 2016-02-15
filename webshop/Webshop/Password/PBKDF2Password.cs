using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Password
{
    /// <summary>
    /// A container object which stores a hash, it's salt and the amounth of iterations.
    /// The underlying hashfunction is pbkdf2.
    /// </summary>
    public sealed class PBKDF2Password : IDisposable
    {
        /// <summary>
        /// the number of bytes the salt is in lenght. make sure that this is bigger than what a graphics card can handle easily to be safe.
        /// </summary>
        public static readonly int SALT_LENGHT = 64;

        /// <summary>
        /// the number of iterations the hashfunction does.
        /// rule of thumb, make it as big as possible whitout annoying your end-users.
        /// </summary>
        public static readonly int ITERATIONS = 64000;

        /// <summary>
        /// The amounth of bytes the hash exists of, again. as big as possible.
        /// but pbkdf2 has a problem that after 20 bytes it doesn't cost as much cpu power for the hacker as it does for the user.
        /// </summary>
        public static readonly int HASH_BYTE_COUNT = 20;

        /// <summary>
        /// A computed hash
        /// </summary>
        public byte[] Hash { get; private set; }  //should be concidered secret, although it's encrypted. thus fairly safe if it leaks.

        /// <summary>
        /// The accompanying salt of the hash.
        /// </summary>
        //public byte[] Salt { get; private set; }  //doesn't have to be concidered secret but it can't hurt

        public byte[] Salt { get; private set; }

        /// <summary>
        /// The amounth of iterations the public hashfunction has to do.
        /// </summary>
        public int Iterations { get; private set; }

        /// <summary>
        /// Makes a Password from old password data.
        /// </summary>
        /// <param name="hash">The already hashed password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">How much iterations this password got.</param>
        public PBKDF2Password(byte[] hash, byte[] salt, int iterations)
        {
            Hash = hash;
            Salt = salt;
            Iterations = iterations;
        }

        /// <summary>
        /// Makes a Password
        /// </summary>
        /// <param name="password">The byte[] representing the password.</param>
        /// <param name="leaveOpen">if no, the byte[] will be disposed after use.</param>
        public PBKDF2Password(byte[] password, bool leaveOpen)
        {
            setIterations();
            gen(password, leaveOpen);
        }

        /// <summary>
        /// Makes a Password from a SecureString
        /// </summary>
        /// <param name="passwordString">The SecureString representing a password.</param>
        /// <param name="leaveOpen">If no, the SecureString will be disposed after use.</param>
        public PBKDF2Password(SecureString passwordString, bool leaveOpen)
        {
            int length = passwordString.Length;
            char[] chars = new char[length];
            IntPtr pointer = IntPtr.Zero;

            try
            {
                pointer = Marshal.SecureStringToBSTR(passwordString);
                Marshal.Copy(pointer, chars, 0, length);
            }
            finally
            {
                if (pointer != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(pointer);
                }
                if (!leaveOpen)
                {
                    passwordString.Dispose();
                }
            }

            byte[] bytes = new byte[chars.Length];
            Buffer.BlockCopy(chars, 0, bytes, 0, bytes.Length);
            for(int i = 0; i < chars.Length; i++)
            {
                chars[i] = '0';
            }

            setIterations();

            this.gen(bytes, false);
        }

        /// <summary>
        /// Unsafe. Use a SecureString.
        /// </summary>
        /// <param name="passwordString"></param>
        public PBKDF2Password(string passwordString)
        {
            setIterations();
            byte[] bytes = new byte[passwordString.Length * sizeof(char)];
            Buffer.BlockCopy(passwordString.ToCharArray(), 0, bytes, 0, bytes.Length);
            gen(bytes, true);
        }

        /// <summary>
        /// Generates the password, including the hash.
        /// </summary>
        /// <param name="password">The data to generate the password from.</param>
        /// <param name="leaveOpen">If no, the SecureString will be disposed after use.</param>
        private void gen(byte[] password, bool leaveOpen)
        {
            Salt = new byte[PBKDF2Password.SALT_LENGHT];

            //generating salt
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(Salt);
            }

            //generating hash
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, Salt, Iterations))
            {
                Hash = pbkdf2.GetBytes(HASH_BYTE_COUNT);
            }

            if (!leaveOpen)
            {
                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = 0;
                }
            }
        }

        public override string ToString()
        {
            return BitConverter.ToString(Hash) + ' ' + BitConverter.ToString(Salt) + ' ' + Iterations;
        }

        public void Dispose()
        {
            Hash = null;
            Salt = null;
            Iterations = 0;
        }

        public void setIterations()
        {
            //double the amounth of iterations every year, starting at 2012.
            Iterations = ITERATIONS * (int)Math.Ceiling(Math.Pow(2, ((DateTime.UtcNow - new DateTime(2012, 1, 1)).TotalDays / 365.25 / 2.0)));
            //adding a little more randomness to it, makes it hard for graphics cards to optimize some stuff. it looks not mutch, but it does a thing.
            Iterations += new Random().Next(2000);

        }
    }
}
