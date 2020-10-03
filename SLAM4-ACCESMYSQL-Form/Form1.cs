using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace SLAM4_ACCESMYSQL_Form
{
    public partial class Form1 : Form
    {
        DBConnect cnx;
        Personne persAvant;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnecter_Click(object sender, EventArgs e)
        {
            string srv = txtHost.Text;
            string DB = txtDB.Text;
            string ID = txtID.Text;
            string MDP = txtMDP.Text;
            cnx = new DBConnect(srv, DB, ID, MDP);
            if(cnx.OpenConnection())
            {
                MessageBox.Show("CONNEXION REUSSIE !!!!!","Avertissement",MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Fermer la connexion
                cnx.CloseConnection();
            }

        }

        private void btnListe_Click(object sender, EventArgs e)
        {
            //Création de la liste de personnes à récupérer => appel de la méthode Select()
            List<Personne> lstPers = cnx.Select();
            foreach(Personne unePers in lstPers)
            {
                dgvPersonnes.Rows.Add(unePers.Nom, unePers.Dnaissance);
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            //Création de la nouvelle personne
            Personne pers = new Personne(txtNom.Text, dtDDN.Value);
            //Ajouter la personne au datagridview
            dgvPersonnes.Rows.Add(pers.Nom, pers.Dnaissance);
            //Ajouter la personne dans la BD
            cnx.Insert(pers);
            //Effacer les données des zones de texte
            txtNom.Clear();
            dtDDN.Value = DateTime.Now;
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            //Récupérer la personne sélectionnée
            int ligne = dgvPersonnes.CurrentRow.Index;
            string leNom = dgvPersonnes.Rows[ligne].Cells[0].Value.ToString();
            DateTime laDDN = DateTime.Parse(dgvPersonnes.Rows[ligne].Cells[1].Value.ToString());
            //Création de l'objet personne
            Personne pers = new Personne(leNom, laDDN);
            //Suppression dans BD
            cnx.Delete(pers);
            //Suppression dans le datagridview
            dgvPersonnes.Rows.RemoveAt(ligne);
        }

        private void btnEditer_Click(object sender, EventArgs e)
        {
            //Récupérer la personne sélectionnée
            int ligneMAJ = dgvPersonnes.CurrentRow.Index;
            string leNom = dgvPersonnes.Rows[ligneMAJ].Cells[0].Value.ToString();
            DateTime laDDN = DateTime.Parse(dgvPersonnes.Rows[ligneMAJ].Cells[1].Value.ToString());
            //Création de l'objet personne avant modification variable globale pour être disponible  pour btn MAJ
            persAvant = new Personne(leNom, laDDN);
            //Mettre les valeurs dans les zones pour modifications
            txtNom.Text = leNom;
            dtDDN.Value = laDDN;
        }

        private void btnMAJ_Click(object sender, EventArgs e)
        {
            //Création de la nouvelle personne
            Personne pers = new Personne(txtNom.Text, dtDDN.Value);
            //Supprimer la ligne sélectionnée
            int ligneMAJ = dgvPersonnes.CurrentRow.Index;
            dgvPersonnes.Rows.RemoveAt(ligneMAJ);
            //Ajouter la personne au datagridview
            dgvPersonnes.Rows.Add(pers.Nom, pers.Dnaissance);
            //Supprimer la personne dans la BD
            cnx.Update(persAvant, pers);
            //Effacer les données des zones de texte
            txtNom.Clear();
            dtDDN.Value = DateTime.Now;
        }
    }
}
