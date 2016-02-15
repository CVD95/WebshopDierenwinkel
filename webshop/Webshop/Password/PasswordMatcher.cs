using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password
{
    public class PasswordMatcher : IDisposable
    {
        private byte[] _hash;
        public byte[] _matchPattern;
        private byte[] _salt;
        private int _iterations;

        private bool? _isMatch;
        public bool LeaveOpen { get; private set; }

        /// <summary>
        /// true if the password and the pattern are a match
        /// </summary>
        public bool IsMatch
        {
            get
            {
                if (_isMatch == null)
                {
                    match();
                }
                return (bool)_isMatch;
            }
        }

        public PasswordMatcher(PBKDF2Password password, SecureString matchPattern, bool leaveOpen)
        {
            _hash = password.Hash;
            _salt = password.Salt;
            _iterations = password.Iterations;
            
            int length = matchPattern.Length;
            char[] chars = new char[length];
            IntPtr pointer = IntPtr.Zero;

            try
            {
                pointer = Marshal.SecureStringToBSTR(matchPattern);
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
                    matchPattern.Dispose();
                }
            }

            Buffer.BlockCopy(chars, 0, _matchPattern, 0, chars.Length);
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = '0';
            }

            LeaveOpen = leaveOpen;
        }

        public PasswordMatcher(byte[] hash, SecureString matchPattern, byte[] salt, int iterations, bool leaveOpen)
        {
            _hash = hash;

            int length = matchPattern.Length;
            char[] chars = new char[length];
            IntPtr pointer = IntPtr.Zero;

            try
            {
                pointer = Marshal.SecureStringToBSTR(matchPattern);
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
                    matchPattern.Dispose();
                }
            }

            Buffer.BlockCopy(chars, 0, _matchPattern, 0, chars.Length);
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = '0';
            }

            _salt = salt;
            _iterations = iterations;
            LeaveOpen = leaveOpen;
        }


        public PasswordMatcher(PBKDF2Password password, string matchPattern, bool leaveOpen)
        {
            _hash = password.Hash;
            _salt = password.Salt;
            _iterations = password.Iterations;

            byte[] bytes = new byte[matchPattern.Length * sizeof(char)];
            Buffer.BlockCopy(matchPattern.ToCharArray(), 0, bytes, 0, bytes.Length);
            _matchPattern = bytes;
            LeaveOpen = leaveOpen;
        }

        public PasswordMatcher(PBKDF2Password password, byte[] matchPattern, bool leaveOpen)
        {
            _hash = password.Hash;
            _salt = password.Salt;
            _iterations = password.Iterations;
            _matchPattern = matchPattern;
            LeaveOpen = leaveOpen;
        }

        public PasswordMatcher(byte[] hash, byte[] matchPattern, byte[] salt, int iterations, bool leaveOpen)
        {
            _hash = hash;
            _matchPattern = matchPattern;
            _salt = salt;
            _iterations = iterations;
            LeaveOpen = leaveOpen;
        }

        public PasswordMatcher(byte[] hash, string matchPattern, byte[] salt, int iterations, bool leaveOpen)
        {
            _hash = hash;
            _salt = salt;
            _iterations = iterations;

            byte[] bytes = new byte[matchPattern.Length * sizeof(char)];
            Buffer.BlockCopy(matchPattern.ToCharArray(), 0, bytes, 0, bytes.Length);
            _matchPattern = bytes;
        }

        private void match()
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(_matchPattern, _salt, _iterations))
            {
                _isMatch = pbkdf2.GetBytes(_hash.Length).SequenceEqual(_hash);
            }
        }

        public void Dispose()
        {
            _hash = null;
            _matchPattern = null;
            _salt = null;
        }

        public void Dispose(bool dispose)
        {
            if (dispose)
            {
                Dispose();
            }
        }
    }
}
