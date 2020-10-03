using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAM4_ACCESMYSQL_Form
{
    public class Personne
    {
        // attributs de l'objet : un par colonne
        private string nom;
        private DateTime dnaissance;

        // méthodes d'accès (get/set)
        public string Nom { get => nom; set => nom = value; }
        public DateTime Dnaissance { get => dnaissance; set => dnaissance = value; }

        //Constructeur
        public Personne(string nom, DateTime madate)
        {
            this.nom = nom;
            this.dnaissance = madate;
        }

        // Méthode de conversion SQL <-> C# (les formats sont différents)
        public String Convert2MySQL(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
