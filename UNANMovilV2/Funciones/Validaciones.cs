using System;
using System.Linq;
using System.Text;

namespace UNANMovilV2.Funciones
{
    public static class Validaciones
    {
        private static readonly Random random = new Random();
        private const string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()";

        public static string GenerarContraseñaAleatoria()
        {
            int longitud = 8; // Longitud mínima de la contraseña
            string caracteres = UpperCaseLetters + LowerCaseLetters + Numbers + SpecialCharacters;

            // Asegurarse de que la contraseña cumpla con los criterios mínimos
            while (true)
            {
                StringBuilder contraseña = new StringBuilder();
                contraseña.Append(RandomCharacter(UpperCaseLetters));
                contraseña.Append(RandomCharacter(LowerCaseLetters));
                contraseña.Append(RandomCharacter(Numbers));
                contraseña.Append(RandomCharacter(SpecialCharacters));

                // Generar caracteres aleatorios adicionales
                for (int i = 4; i < longitud; i++)
                {
                    contraseña.Append(RandomCharacter(caracteres));
                }

                // Mezclar los caracteres aleatoriamente
                contraseña = new StringBuilder(new string(contraseña.ToString().ToCharArray().OrderBy(c => random.Next()).ToArray()));

                // Comprobar si la contraseña cumple con los criterios
                if (ContraseñaCumpleCriterios(contraseña.ToString()))
                {
                    return contraseña.ToString();
                }
            }
        }
        private static char RandomCharacter(string caracteres)
        {
            int indice = random.Next(0, caracteres.Length);
            return caracteres[indice];
        }

        public static bool ContraseñaCumpleCriterios(string contraseña)
        {
            bool mayuscula = false, minuscula = false, numero = false, caracespecial = false;

            foreach (char c in contraseña)
            {
                if (char.IsUpper(c))
                {
                    mayuscula = true;
                }
                else if (char.IsLower(c))
                {
                    minuscula = true;
                }
                else if (char.IsDigit(c))
                {
                    numero = true;
                }
                else if (SpecialCharacters.Contains(c))
                {
                    caracespecial = true;
                }
            }
            return mayuscula && minuscula && numero && caracespecial;
        }
    }
}
