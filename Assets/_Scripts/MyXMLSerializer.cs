using UnityEngine;
using System.Collections;

using System.Security.Cryptography;
using System.Text;
using System.IO;

using System.Xml.Serialization;

using Constant;

namespace Special.Saver
{
	
	public class MyXMLSerializer : Object
	{
		private const string key = "HDsdNNhp";

		private const string IV = "gnc_DADA";

		private static DESCryptoServiceProvider DESCSP = new DESCryptoServiceProvider ();

		public static void EncryptAndSerialize <T> ( string filename, T obj )
		{

			using ( FileStream fs = File.Open ( filename, FileMode.Create ) )
			{
				using ( CryptoStream cs = new CryptoStream ( fs, DESCSP.CreateEncryptor ( Encoding.ASCII.GetBytes ( key ), Encoding.ASCII.GetBytes ( IV ) ), CryptoStreamMode.Write ) )
				{
					XmlSerializer xmlser = new XmlSerializer ( typeof ( T ) );
					xmlser.Serialize ( cs, obj ); 
				}
			}
		}

		public static T DecryptAndDeserialize <T> ( string filename )
		{
			using ( FileStream fs = File.Open ( filename, FileMode.Open ) )
			{
				using ( CryptoStream cs = new CryptoStream ( fs, DESCSP.CreateDecryptor ( Encoding.ASCII.GetBytes ( key ), Encoding.ASCII.GetBytes ( IV ) ), CryptoStreamMode.Read ) )
				{
					XmlSerializer xmlser = new XmlSerializer ( typeof ( T ) );
					return ( T ) xmlser.Deserialize ( cs );
				}
			}
		}

		public static void Serialize <T> ( string filename, T obj )
		{

			using ( FileStream fs = File.Open ( filename, FileMode.Create ) )
			{
				XmlSerializer xmlser = new XmlSerializer ( typeof ( T ) );
				xmlser.Serialize ( fs, obj ); 
			}
		}

		public static T Deserialize <T> ( string filename )
		{
			using ( FileStream fs = File.Open ( filename, FileMode.Open ) )
			{
				XmlSerializer xmlser = new XmlSerializer ( typeof ( T ) );
				return ( T ) xmlser.Deserialize ( fs );
			}
		}

		//	public static void EncryptAndSerialize<T> ( string filename, T obj, string encryptionKey )
		//	{
		//		var key = new DESCryptoServiceProvider ();
		//		var e = key.CreateEncryptor ( Encoding.ASCII.GetBytes ( "64bitPas" ), Encoding.ASCII.GetBytes ( encryptionKey ) );
		//		using ( var fs = File.Open ( filename, FileMode.Create ) )
		//		using ( var cs = new CryptoStream ( fs, e, CryptoStreamMode.Write ) )
		//			( new XmlSerializer ( typeof ( T ) ) ).Serialize ( cs, obj );
		//	}
		//
		//	public static T DecryptAndDeserialize<T> ( string filename, string encryptionKey )
		//	{
		//		var key = new DESCryptoServiceProvider ();
		//		var d = key.CreateDecryptor ( Encoding.ASCII.GetBytes ( "64bitPas" ), Encoding.ASCII.GetBytes ( encryptionKey ) );
		//		using ( var fs = File.Open ( filename, FileMode.Open ) )
		//		using ( var cs = new CryptoStream ( fs, d, CryptoStreamMode.Read ) )
		//			return ( T ) ( new XmlSerializer ( typeof ( T ) ) ).Deserialize ( cs );
		//	}

		public static void ResetProgress ()
		{
			File.Delete (Application.persistentDataPath + Constants.ProgressFileName);
		}

	}

}
