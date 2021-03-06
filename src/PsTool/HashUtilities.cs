// --------------------------------------------------------------------------------------------
// <copyright file="HashUtilities.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace SIL.Tool
{
	#region cryptography class...

	/// <summary>Contains methods and properties for two-way encryption and decryption</summary>
	/// <author>Samdoss</author>
	/// <written>Oct 08, 2008</written>
	/// <Updated>Oct 08, 2008</Updated>
	public class HashUtilities
	{
		#region Private members...

		private string mKey = string.Empty;
		private string mSalt = string.Empty;
		private readonly ServiceProviderEnum _mAlgorithm;
		private readonly SymmetricAlgorithm _mCryptoService;

		private void SetLegalIV()
		{
			// Set symmetric algorithm
			switch(_mAlgorithm)
			{
				case ServiceProviderEnum.Rijndael:
					_mCryptoService.IV = new byte[] {0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73,0xcc};
					break;
				default:
					_mCryptoService.IV = new byte[] {0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9};
					break;
			}
		}

		#endregion

		#region Public interfaces...

		public enum ServiceProviderEnum: int
		{
			// Supported service providers
			Rijndael,
			RC2,
			DES,
			TripleDES
		}
				
		public HashUtilities()
		{
			// Default symmetric algorithm
			_mCryptoService = new RijndaelManaged();
			_mCryptoService.Mode = CipherMode.CBC;
			_mAlgorithm = ServiceProviderEnum.Rijndael;
		}

		public HashUtilities(ServiceProviderEnum serviceProvider)
		{	
			// Select symmetric algorithm
			switch(serviceProvider)
			{
				case ServiceProviderEnum.Rijndael:
					_mCryptoService = new RijndaelManaged();
					_mAlgorithm = ServiceProviderEnum.Rijndael;
					break;
				case ServiceProviderEnum.RC2:
					_mCryptoService = new RC2CryptoServiceProvider();
					_mAlgorithm = ServiceProviderEnum.RC2;
					break;
				case ServiceProviderEnum.DES:
					_mCryptoService = new DESCryptoServiceProvider();
					_mAlgorithm = ServiceProviderEnum.DES;
					break;
				case ServiceProviderEnum.TripleDES:
					_mCryptoService = new TripleDESCryptoServiceProvider();
					_mAlgorithm = ServiceProviderEnum.TripleDES;
					break;
			}
			_mCryptoService.Mode = CipherMode.CBC;
		}

        public HashUtilities(string serviceProviderName)
		{
			try
			{
				// Select symmetric algorithm
				switch(serviceProviderName.ToLower())
				{
					case "rijndael":
						serviceProviderName = "Rijndael"; 
						_mAlgorithm = ServiceProviderEnum.Rijndael;
						break;
					case "rc2":
						serviceProviderName = "RC2";
						_mAlgorithm = ServiceProviderEnum.RC2;
						break;
					case "des":
						serviceProviderName = "DES";
						_mAlgorithm = ServiceProviderEnum.DES;
						break;
					case "tripledes":
						serviceProviderName = "TripleDES";
						_mAlgorithm = ServiceProviderEnum.TripleDES;
						break;
				}

				// Set symmetric algorithm
				_mCryptoService = (SymmetricAlgorithm)CryptoConfig.CreateFromName(serviceProviderName);
				_mCryptoService.Mode = CipherMode.CBC;
			}
			catch
			{
				throw;
			}
		}

		public virtual byte[] GetLegalKey()
		{
			// Adjust key if necessary, and return a valid key
			if (_mCryptoService.LegalKeySizes.Length > 0)
			{
				// Key sizes in bits
				int keySize = mKey.Length * 8;
				int minSize = _mCryptoService.LegalKeySizes[0].MinSize;
				int maxSize = _mCryptoService.LegalKeySizes[0].MaxSize;
				int skipSize = _mCryptoService.LegalKeySizes[0].SkipSize;
				
				if (keySize > maxSize)
				{
					// Extract maximum size allowed
					mKey = mKey.Substring(0, maxSize / 8);
				}
				else if (keySize < maxSize)
				{
					// Set valid size
					int validSize = (keySize <= minSize)? minSize : (keySize - keySize % skipSize) + skipSize;
					if (keySize < validSize)
					{
						// Pad the key with asterisk to make up the size
						mKey = mKey.PadRight(validSize / 8, '*');
					}
				}
			}
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(mKey, ASCIIEncoding.ASCII.GetBytes(mSalt));
			return key.GetBytes(mKey.Length);
		}

		public virtual string Encrypt(string plainText)
		{
			byte[] plainByte = ASCIIEncoding.ASCII.GetBytes(plainText);
			byte[] keyByte = GetLegalKey();

			// Set private key
			_mCryptoService.Key = keyByte;
			SetLegalIV();
			
			// Encryptor object
			ICryptoTransform cryptoTransform = _mCryptoService.CreateEncryptor();
			
			// Memory stream object
			MemoryStream ms = new MemoryStream();

			// Crpto stream object
			CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);

			// Write encrypted byte to memory stream
			cs.Write(plainByte, 0, plainByte.Length);
			cs.FlushFinalBlock();

			// Get the encrypted byte length
			byte[] cryptoByte = ms.ToArray();

			// Convert into base 64 to enable result to be used in Xml
			return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
		}
		
		public virtual string Decrypt(string cryptoText)
		{
			// Convert from base 64 string to bytes
			byte[] cryptoByte = Convert.FromBase64String(cryptoText);
			byte[] keyByte = GetLegalKey();

			// Set private key
			_mCryptoService.Key = keyByte;
			SetLegalIV();

			// Decryptor object
			ICryptoTransform cryptoTransform = _mCryptoService.CreateDecryptor();
			try
			{
				// Memory stream object
				MemoryStream ms = new MemoryStream(cryptoByte, 0, cryptoByte.Length);

				// Crpto stream object
				CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read);

				// Get the result from the Crypto stream
				StreamReader sr = new StreamReader(cs);
				return sr.ReadToEnd();
			}
			catch
			{
				return null;
			}
		}

		public string Key
		{
			get
			{
				return mKey;
			}
			set
			{
				mKey = value;
			}
		}

		public string Salt
		{
			// Salt value
			get
			{
				return mSalt;
			}
			set
			{
				mSalt = value;
			}
		}
		#endregion
	}
	#endregion
}

