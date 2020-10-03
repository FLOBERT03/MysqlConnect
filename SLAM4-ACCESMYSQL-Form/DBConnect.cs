using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SLAM4_ACCESMYSQL_Form
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructeurs
        public DBConnect()
        { }

        public DBConnect(string srv, string DB, string ID, string MDP)
        {
            server = srv;
            database = DB;
            uid = ID;
            password = MDP;
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            //Instanciation de la connexion à la BD sur le serveur en localhost ici
            connection = new MySqlConnection(connectionString);
        }

        //Ouverture de la connexion à la BD
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Connexion au serveur impossible !  Contactez l'administrateur");
                        break;

                    case 1045:
                        MessageBox.Show("Login et/ou mot de passe invalide(s). Réessayez");
                        break;
                }
                return false;
            }
        }

        //Fermeture de la connexion
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insertion d'un enregistrement
        public void Insert(Personne unePers)
        {
            string query = "INSERT INTO matable (nom, ddn) VALUES('" + unePers.Nom + "', '" + unePers.Convert2MySQL(unePers.Dnaissance) +"')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                
                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }


        //Mise à jour d'un enregistrement
        public void Update(Personne ancPers, Personne nvPers)
        {
            string query = "UPDATE matable SET nom='" + nvPers.Nom + "', ddn='" + nvPers.Convert2MySQL(nvPers.Dnaissance) + "' WHERE nom='" + ancPers.Nom +"'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }



        //Suppression d'un enregistrement
        public void Delete(Personne unePers)
        {
            string query = "DELETE FROM matable WHERE nom='" + unePers.Nom + "'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        
        //Lecture des enregistrements
        public List<Personne> Select()
        {
            Personne ligne;
            string query = "SELECT * FROM matable";

            //Création de la liste des personnes
            List<Personne> list = new List<Personne>();

            //Connexion à la BD
            if (this.OpenConnection() == true)
            {
                //Création de la requête SQL 
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Exécution de la requête
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //Lecture des enregistrement et stockage dans la liste
                while (dataReader.Read())
                {
                    ligne = new Personne(dataReader["nom"].ToString(), Convert2C(dataReader["ddn"].ToString()));
                    list.Add(ligne) ;
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        private DateTime Convert2C(String sqlDate)
        {
            return DateTime.Parse(sqlDate);
        }

        /*
        //Nb d'enregistrements
        public int Count()
        {
        }

        //Backup
        public void Backup()
        {
        }

        //Restauration
        public void Restore()
        {
        }*/
    }
}
