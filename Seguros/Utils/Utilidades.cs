namespace Seguros.Utils
{
    public class Utilidades
    {
        public static string Formato(string cadena)
        {
            string[] nombre = cadena.Trim().Split(' ');
            for (int i = 0; i < nombre.Length; i++)
            {
                nombre[i] = nombre[i][0].ToString().ToUpper() + nombre[i].Substring(1).ToLower();
            }

            return string.Join(" ", nombre);
        }
    }
}