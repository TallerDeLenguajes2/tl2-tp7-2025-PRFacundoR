using Microsoft.Data.Sqlite;

public class PresupuestosRepository
{
    private string _coneccionADB = "Data Source=DB/nueva.db";

    public List<Presupuestos> GetAllPresupuestos()
    {
        string sql_query = "SELECT * FROM presupuestos INNER JOIN presupuestoDetalle using(idPresupuesto) JOIN productos ON id_prod=idProducto";
        List<Presupuestos> presupuestos = new List<Presupuestos>();

        using var conecttion = new SqliteConnection(_coneccionADB);
        conecttion.Open();

        var comando = new SqliteCommand(sql_query, conecttion);

        using (SqliteDataReader reader = comando.ExecuteReader())
        {
            /*while (reader.Read())
            {
                int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);

                var presupuesto = presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);
                if (presupuesto == null)// Verifico si ya existe,Evitar duplicar presupuestos, luego ver sin if
                {
                    var presupuest = new Presupuestos();
                    presupuest.IdPresupuesto = idPresupuesto;
                    presupuest.NombreDestinatario = reader["nombreDestinatario"].ToString();
                    presupuest.FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]);

                    var produc = new Productos();
                    produc.IdProducto = Convert.ToInt32(reader["id_prod"]);
                    produc.Descripcion = reader["descripcion"].ToString();
                    produc.Precio = Convert.ToInt32(reader["precio"]);

                    var presuDetalle = new PresupuestoDetalle();
                    presuDetalle.Producto = produc;
                    presuDetalle.Cantidad = Convert.ToInt32(reader["cantidad"]);

                    presupuest.Detalle = new List<PresupuestoDetalle>();
                    presupuest.Detalle.Add(presuDetalle);

                    presupuestos.Add(presupuest);

                }

            }*/

            while (reader.Read())
            {
                int idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                var presupuesto = presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);

                if (presupuesto == null)
                {
                    presupuesto = new Presupuestos
                    {
                        IdPresupuesto = idPresupuesto,
                        NombreDestinatario = reader["nombreDestinatario"].ToString(),
                        FechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]),
                        Detalle = new List<PresupuestoDetalle>()
                    };
                    presupuestos.Add(presupuesto);
                }

                var producto = new Productos
                {
                    IdProducto = Convert.ToInt32(reader["id_prod"]),
                    Descripcion = reader["descripcion"].ToString(),
                    Precio = Convert.ToInt32(reader["precio"])
                };

                var detalle = new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = Convert.ToInt32(reader["cantidad"])
                };

                presupuesto.Detalle.Add(detalle);
            }


            //conecttion.Close(); no haria falta ya que usamos using
            return presupuestos;
        }
    }



    public int InsertarPresupuesto(Presupuestos presupuesto)
    {
        int nuevoID = 0;
        using (SqliteConnection coneccion = new SqliteConnection(_coneccionADB))
        {
            coneccion.Open();
            string sql = "INSERT INTO presupuesto (nombreDestinario, fechaCreacion) VALUES (@dest, @fecha); SELECT last_insert_rowid();";

            using (var comando = new SqliteCommand(sql, coneccion))
            {

                comando.Parameters.Add(new SqliteParameter("@dest", presupuesto.NombreDestinatario));
                comando.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));
                nuevoID = Convert.ToInt32(comando.ExecuteScalar());
            }
            coneccion.Close();
        }

        return nuevoID;
    }


    // public int ActualizarProducto(int idProduc, Presupuestos presupuesto)
    // {
    //     int filasAfectadas = 0;

    //     using (SqliteConnection conexion = new SqliteConnection(_coneccionADB))
    //     {
    //         conexion.Open();

    //         string sql = "UPDATE presupuesto SET precio = @precio WHERE id_prod = @idProduc;";

    //         using (var comando = new SqliteCommand(sql, conexion))
    //         {
    //             comando.Parameters.AddWithValue("@precio", produc.Precio);
    //             comando.Parameters.AddWithValue("@idProduc", idProduc);

    //             filasAfectadas = comando.ExecuteNonQuery();
    //         }
    //     }

    //     return filasAfectadas;
    // }

    public void borrarPresupuesto(int id)
    {
        using (var conexion = new SqliteConnection(_coneccionADB))
        {
            conexion.Open();

            string sql = "DELETE FROM presupuesto WHERE idPresupuesto = @id";
            using (var comando = new SqliteCommand(sql, conexion))
            {
                comando.Parameters.Add(new SqliteParameter("@id", id));
                comando.ExecuteNonQuery();
            }
        }
    }

    /* public Presupuestos ObtenerProductoPorId(int id)
     {
         using (var conexion = new SqliteConnection(_coneccionADB))
         {
             conexion.Open();
             string sql = "SELECT * FROM producto WHERE id_prod=@id";
             using (var comando = new SqliteCommand(sql, conexion))
             {
                 // Parámetro para evitar inyección SQL
                 comando.Parameters.AddWithValue("@id", id);
                 using (var lector = comando.ExecuteReader())
                 {
                     if (lector.Read()) // si encontró el producto
                     {
                         var producto = new Presupuestos
                         {
                             IdProducto = lector.GetInt32(0),              // o lector["id_prod"]
                             Descripcion = lector.GetString(1),         // lector["nombre"]
                             Precio = lector.GetInt32(2),        // lector["precio"]

                         };

                         return producto;
                     }
                     else
                     {
                         return null; // no se encontró
                     }
                 }

             }

         }
     }*/
}