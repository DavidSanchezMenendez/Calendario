using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Calendario
{

    public partial class Form1 : Form
    {
        private string connectionString = "Server=192.168.1.93;Database=david;Uid=usuarioRemoto;Pwd=1234;";
        Random random = new Random();
        private DateTime selectedDate;
        private TextBox txtEvent;
        private MonthCalendar monthCalendar;
        private Button btnSaveEvent;
        private Button btnDelete;
        private Button subirFotos;
        private ListBox lstEvents;
        PictureBox pictureBox;
        PictureBox pictureBox1;
        PictureBox pictureBox2;
        List<PictureBox> pic;
        List<int>  numeroRepetido = new List<int>();
        private Dictionary<DateTime, string> eventsDictionary;

        public Form1()
        {
            InitializeComponent();

            selectedDate = DateTime.Today;
            eventsDictionary = new Dictionary<DateTime, string>();
        }

        private void InitializeComponent()
        {
           // InsertarImagen();
             pictureBox = new PictureBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();


            // Establece la ubicación y el tamaño del PictureBox
           
            pictureBox.Location = new Point(700, 0);
            pictureBox.Size = new Size(500, 500);

            pictureBox1.Location = new Point(700, 200);
            pictureBox1.Size = new Size(500, 500);

            pictureBox2.Location = new Point(200, 600);
            pictureBox2.Size = new Size(500, 500);

            // Establece la imagen del PictureBox
            //pictureBox.Image = Image.FromFile(@"C:\Users\David\Pictures\goodshit\FwqY1STWwAQBrqY.jpeg");

            // Añade el PictureBox al formulario
            






            this.txtEvent = new System.Windows.Forms.TextBox();
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.btnSaveEvent = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.subirFotos = new System.Windows.Forms.Button();



            this.lstEvents = new System.Windows.Forms.ListBox();
            this.SuspendLayout();

            // txtEvent
            this.txtEvent.Location = new System.Drawing.Point(12, 202);
            this.txtEvent.Multiline = true;
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.Size = new System.Drawing.Size(180, 50);
            this.txtEvent.TabIndex = 0;

            // monthCalendar
            this.monthCalendar.Location = new System.Drawing.Point(12, 12);
            this.monthCalendar.MaxSelectionCount = 1;
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 1;
            this.monthCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateSelected);

            // btnSaveEvent
            this.btnSaveEvent.Location = new System.Drawing.Point(198, 202);
            this.btnSaveEvent.Name = "btnSaveEvent";
            this.btnSaveEvent.Size = new System.Drawing.Size(75, 23);
            this.btnSaveEvent.TabIndex = 2;
            this.btnSaveEvent.Text = "Save Event";
            this.btnSaveEvent.UseVisualStyleBackColor = true;
            this.btnSaveEvent.Click += new System.EventHandler(this.btnSaveEvent_Click);

            //btDelete
            this.btnDelete.Location = new System.Drawing.Point(300, 202);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete Envent";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.DeleteEvent);


            //subirFotos
            this.subirFotos.Location = new System.Drawing.Point(300, 240);
            this.subirFotos.Name = "subirFotos";
            this.subirFotos.Size = new System.Drawing.Size(75, 23);
            this.subirFotos.TabIndex = 2;
            this.subirFotos.Text = "subirFotos";
            this.subirFotos.UseVisualStyleBackColor = true;
            this.subirFotos.Click += new System.EventHandler(this.SubirImagen);



            // lstEvents
            this.lstEvents.FormattingEnabled = true;
            this.lstEvents.Location = new System.Drawing.Point(198, 12);
            this.lstEvents.Name = "lstEvents";
            this.lstEvents.Size = new System.Drawing.Size(180, 186);
            this.lstEvents.TabIndex = 3;

            // Form1
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.lstEvents);
            this.Controls.Add(this.btnSaveEvent);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.txtEvent);
            this.Controls.Add(this.subirFotos);
            this.Name = "Form1";
            this.Text = "Calendar";
            this.ResumeLayout(false);
            this.PerformLayout();
            Select();
            MostrarImagenMYSQL();
        }
        private void MostrarImagenMYSQL()
        {

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT DatosImagen FROM Fotos WHERE Id = @Id"; // Reemplaza con tu consulta SQL
                    string queryCount = "SELECT COUNT(*) FROM Fotos";
                    MySqlCommand command = new MySqlCommand(queryCount, connection);

                    connection.Open();
                    int cantidadFotos = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();






                    for (int i = 0; i < 3; i++)
                    {
                        
                        connection.Open();
                        MySqlCommand command1 = new MySqlCommand(query, connection);
                        int numeroAleatorio = random.Next(0, cantidadFotos);
                       
                        while (numeroRepetido.Contains(numeroAleatorio))
                        {
                            numeroAleatorio=random.Next(0, cantidadFotos);
                        }
                        numeroRepetido.Add(numeroAleatorio);




                        command1.Parameters.AddWithValue("@Id", numeroAleatorio); // Reemplaza con el ID de la imagen que deseas recuperar




                        byte[] imageData = (byte[])command1.ExecuteScalar();

                        connection.Close();

                        if (imageData != null && imageData.Length > 0)
                        {
                            using (MemoryStream stream = new MemoryStream(imageData))
                            {
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Location = new Point(200 * i, 300);
                                pictureBox.Size = new Size(150, 150);
                                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox.Image = Image.FromStream(stream);

                               
                                Controls.Add(pictureBox);



                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
               

            }


            }
        
        private void SubirImagen(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg, *.png)|*.jpg;*.png|Todos los archivos (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string rutaArchivo = openFileDialog.FileName;
                byte[] archivoBytes = File.ReadAllBytes(rutaArchivo);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO Fotos (DatosImagen) VALUES (@DatosArchivo)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    //command.Parameters.AddWithValue("@Nombre", Path.GetFileName(rutaArchivo));
                    command.Parameters.AddWithValue("@DatosArchivo", archivoBytes);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("ImagenSubida");
                }

                
            }
        }
        private void InsertarImagen()
        {
            byte[] imageData;
            using (FileStream fs = new FileStream(@"C:\Users\David\Pictures\goodshit\1663779264535.jpeg", FileMode.Open, FileAccess.Read))
            {
            imageData = new byte[fs.Length];
            fs.Read(imageData, 0, (int)fs.Length);
            }
            using (MySqlConnection connection = new MySqlConnection(connectionString)) // Reemplaza con tu cadena de conexión
            {
                string query = "INSERT INTO fotos (DatosImagen) VALUES (@DatosImagen)"; // Reemplaza con el nombre de tu tabla y columna

                // Configurar el comando SQL
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DatosImagen", imageData);

                // Abrir la conexión y ejecutar el comando
                connection.Open();
                command.ExecuteNonQuery();

                // Cerrar la conexión
                connection.Close();
            }
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            selectedDate = e.Start;
            if (eventsDictionary.ContainsKey(selectedDate))
            {
                txtEvent.Text = eventsDictionary[selectedDate];
            }
            else
            {
                txtEvent.Text = string.Empty;
            }
        }

        private void btnSaveEvent_Click(object sender, EventArgs e)
        {
            string eventText = txtEvent.Text;
            if (eventsDictionary.ContainsKey(selectedDate))//Hay algo guardado en el Dictionary?
            {
                eventsDictionary[selectedDate] = eventText;
            }
            else
            {
                eventsDictionary.Add(selectedDate, eventText);
            }
            btnSave_Click();
            

            UpdateEventsListBox();
        }

        private void UpdateEventsListBox()
        {
            lstEvents.Items.Clear();
            foreach (KeyValuePair<DateTime, string> kvp in eventsDictionary)
            {
                lstEvents.Items.Add(kvp.Key.ToString("dd/MM/yyyy") + ": " + kvp.Value);
            }
            Select();
        }
        private void btnSave_Click()
        {
            // Obtener el número ingresado por el usuario

            
            string datos = txtEvent.Text;
            /*
            if (!string.IsNullOrWhiteSpace(datos))
            {

                string verificaion = "SELECT EXISTS(SELECT 1 FROM datos WHERE nombre = @condicion)";
                string sqlUpdate = "UPDATE eventos SET evento = @Texto   WHERE fecha = @fecha";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand verificacionCommand = new MySqlCommand(verificaion, connection))
                    {
                        verificacionCommand.Parameters.AddWithValue("@condicion", datos);//si condicion es = nombre
                        int existencia = Convert.ToInt32(verificacionCommand.ExecuteScalar());

                        if (existencia > 0)
                        {
                            // Realizar la actualización
                            using (MySqlCommand updateCommand = new MySqlCommand(sqlUpdate, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@condicion", datos);//actializa where nombre sea igual a nombre

                                int filasActualizadas = updateCommand.ExecuteNonQuery();
                                // Aquí puedes realizar cualquier acción adicional después de la actualización
                            }
                            connection.Close();
                        }
                        else
                        {

                        }*/
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {


                    connection.Open();
                    string insert = "INSERT INTO Eventos (Fecha, Evento)VALUES(@Fecha,@Datos)";
                    using (MySqlCommand command = new MySqlCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@Fecha", selectedDate);
                        command.Parameters.AddWithValue("@Datos", datos);

                        int rowsAffected = command.ExecuteNonQuery();
                        // MessageBox.Show("Número guardado en la base de datos. Filas afectadas: ");

                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción de la conexión a la base de datos
                MessageBox.Show("No puedes usar 2 fechas Iguales:");
            }







        }


                
            
            // btn_Select(sender, e);
        
        private void Select()
        {

           
                string selectQuery = "Select * From Eventos ORDER BY Fecha DESC";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Crear el comando SQL para obtener los datos
                    using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                    {
                        // Abrir la conexión
                        connection.Open();
                    lstEvents.Items.Clear();

                    // Crear un StringBuilder para almacenar los eventos
                    StringBuilder sb = new StringBuilder();

                        // Ejecutar el comando y leer los datos
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            // Obtener el valor del campo "Evento" en cada fila
                            string fecha = reader.GetString("Fecha");

                            // Obtener el valor de la columna "Evento"
                            string evento = reader.GetString("Evento");

                            // Agregar la fecha y el evento al StringBuilder
                            string textoEvento = $"{fecha}: {evento}";
                            lstEvents.Items.Add(textoEvento);
                        }
                    }
                    
                    // Cerrar la conexión
                    connection.Close();

                        // Asignar los eventos al Text property del TextBox
                       
                    }
                
            }
        }
        private void DeleteEvent(object sender, EventArgs e)
        {
            string sqlDelete = "DELETE FROM Eventos WHERE Fecha = @Fecha";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
               
                    using (MySqlCommand command = new MySqlCommand(sqlDelete, connection))
                    {
                    command.Parameters.AddWithValue("@Fecha", selectedDate);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    // MessageBox.Show("Número guardado en la base de datos. Filas afectadas: ");

                }
                UpdateEventsListBox();
                connection.Close();
              

                }

            }

        }
}


    