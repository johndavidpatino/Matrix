using System.Security.Cryptography;
using System.Text;

namespace MatrixNext.Data.Utilities;

/// <summary>
/// Servicio de encriptacion compatible con la logica del sistema legado (Encripcion.vb).
/// Soporta multiples algoritmos: DES, TripleDES, RC2, Rijndael, AES.
/// 
/// Nota: Los algoritmos DES, TripleDES, RC2 estan deprecados en .NET moderno,
/// pero se mantienen para compatibilidad con contrasenas heredadas del sistema.
/// 
/// Modo: 1 = Encriptar, 2 = Desencriptar
/// Algoritmo: 1 = DES, 2 = TripleDES, 3 = RC2, 4 = Rijndael, 5 = AES
/// </summary>
public class EncryptionService
{
    private const int ENCRYPT_MODE = 1;
    private const int DECRYPT_MODE = 2;

    /// <summary>
    /// Encripta o desencripta una cadena usando el algoritmo y clave especificados.
    /// Compatible con la funcion Cifrado(modo, algoritmo, cadena, key, vecI) del sistema legado.
    /// </summary>
    public static string Cifrado(int modo, int algoritmo, string cadena, string key, string vecI)
    {
        byte[] plaintext = modo == ENCRYPT_MODE
            ? Encoding.ASCII.GetBytes(cadena)
            : Convert.FromBase64String(cadena);

        byte[] keyBytes = Encoding.ASCII.GetBytes(key);
        byte[] ivBytes = Encoding.ASCII.GetBytes(vecI);

        using (var memdata = new MemoryStream())
        using (ICryptoTransform? transforma = ObtenerTransformacion(modo, algoritmo, keyBytes, ivBytes))
        {
            if (transforma == null)
                throw new ArgumentException($"Algoritmo no soportado: {algoritmo}");

            using (var encstream = new CryptoStream(memdata, transforma, CryptoStreamMode.Write))
            {
                encstream.Write(plaintext, 0, plaintext.Length);
                encstream.FlushFinalBlock();
            }

            if (modo == ENCRYPT_MODE)
                return Convert.ToBase64String(memdata.ToArray());
            else
                return Encoding.ASCII.GetString(memdata.ToArray());
        }
    }

    /// <summary>
    /// Obtiene el transformador criptografico segun el algoritmo especificado.
    /// </summary>
    private static ICryptoTransform? ObtenerTransformacion(int modo, int algoritmo, byte[] key, byte[] iv)
    {
        return algoritmo switch
        {
            1 => ObtenerTransformacionDES(modo, key, iv),
            2 => ObtenerTransformacionTripleDES(modo, key, iv),
            3 => ObtenerTransformacionRC2(modo, key, iv),
            4 => ObtenerTransformacionRijndael(modo, key, iv),
            5 => ObtenerTransformacionAES(modo, key),
            _ => null
        };
    }

    private static ICryptoTransform ObtenerTransformacionDES(int modo, byte[] key, byte[] iv)
    {
        using var des = DES.Create() ?? throw new InvalidOperationException("DES no disponible");
        des.Mode = CipherMode.CBC;
        return modo == ENCRYPT_MODE
            ? des.CreateEncryptor(key, iv)
            : des.CreateDecryptor(key, iv);
    }

    private static ICryptoTransform ObtenerTransformacionTripleDES(int modo, byte[] key, byte[] iv)
    {
        using var des3 = TripleDES.Create() ?? throw new InvalidOperationException("TripleDES no disponible");
        des3.Mode = CipherMode.CBC;
        return modo == ENCRYPT_MODE
            ? des3.CreateEncryptor(key, iv)
            : des3.CreateDecryptor(key, iv);
    }

    private static ICryptoTransform ObtenerTransformacionRC2(int modo, byte[] key, byte[] iv)
    {
        using var rc2 = RC2.Create() ?? throw new InvalidOperationException("RC2 no disponible");
        rc2.Mode = CipherMode.CBC;
        return modo == ENCRYPT_MODE
            ? rc2.CreateEncryptor(key, iv)
            : rc2.CreateDecryptor(key, iv);
    }

    #pragma warning disable SYSLIB0022
    private static ICryptoTransform ObtenerTransformacionRijndael(int modo, byte[] key, byte[] iv)
    {
        using var rj = Rijndael.Create() ?? throw new InvalidOperationException("Rijndael no disponible");
        rj.Mode = CipherMode.CBC;
        return modo == ENCRYPT_MODE
            ? rj.CreateEncryptor(key, iv)
            : rj.CreateDecryptor(key, iv);
    }
    #pragma warning restore SYSLIB0022

    private static ICryptoTransform ObtenerTransformacionAES(int modo, byte[] key)
    {
        using var aes = Aes.Create() ?? throw new InvalidOperationException("AES no disponible");
        aes.Mode = CipherMode.ECB;
        byte[] iv = new byte[aes.BlockSize / 8];
        return modo == ENCRYPT_MODE
            ? aes.CreateEncryptor(key, iv)
            : aes.CreateDecryptor(key, iv);
    }

    /// <summary>
    /// Encripta una contrasena del usuario usando TripleDES (algoritmo 2).
    /// Usa la clave y vector de inicializacion del sistema legado: "Ipsos*23432_2013".
    /// </summary>
    public static string EncryptPassword(string plainPassword)
    {
        const string key = "Ipsos*23432_2013";
        const string iv = "Ipsos*23432_2013";
        return Cifrado(ENCRYPT_MODE, 2, plainPassword, key, iv);
    }

    /// <summary>
    /// Desencripta una contrasena del usuario.
    /// </summary>
    public static string DecryptPassword(string encryptedPassword)
    {
        const string key = "Ipsos*23432_2013";
        const string iv = "Ipsos*23432_2013";
        return Cifrado(DECRYPT_MODE, 2, encryptedPassword, key, iv);
    }

    /// <summary>
    /// Valida una contrasena contra su version encriptada.
    /// </summary>
    public static bool VerifyPassword(string plainPassword, string encryptedPassword)
    {
        return EncryptPassword(plainPassword) == encryptedPassword;
    }
}
