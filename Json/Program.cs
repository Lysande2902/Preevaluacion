using System;
using System.IO;
using System.Text.Json;

class Producto
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
}

class Inventario
{
    private const int MaxProductos = 5;
    private static Producto[] productos = new Producto[MaxProductos];
    private static int cantidadProductos = 0;
    private const string ArchivoInventario = "inventario.json";

    public static void CargarInventario()
    {
        if (File.Exists(ArchivoInventario)) //File.Exists verifica si el archivo existe
        {
            try
            {
                string json = File.ReadAllText(ArchivoInventario); //Lee el archivo
                Producto[] productosCargados = JsonSerializer.Deserialize<Producto[]>(json); //El archivo se convierte en un arreglo de productos

                // Copiar los productos cargados al arreglo principal
                int copiarHasta = Math.Min(productosCargados.Length, MaxProductos);
                for (int i = 0; i < copiarHasta; i++) //Recorre los productos
                {
                    productos[i] = productosCargados[i]; //Guarda los productos
                }
                cantidadProductos = copiarHasta;
            }
            catch (Exception ex) //Si hay un error lo muestra
            {
                Console.WriteLine($"Error al cargar el inventario: {ex.Message}");
            }
        }
    }

    public static void GuardarInventario()
    {
        try
        {
            // Crear un arreglo temporal con solo los productos existentes
            Producto[] productosAGuardar = new Producto[cantidadProductos];
            Array.Copy(productos, productosAGuardar, cantidadProductos);

            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(productosAGuardar, opciones);
            File.WriteAllText(ArchivoInventario, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar el inventario: {ex.Message}");
        }
    }

    public static void AgregarProducto()
    {
        if (cantidadProductos >= MaxProductos)
        {
            Console.WriteLine("Inventario lleno. No se pueden agregar más productos.\n");
            return;
        }

        Producto nuevo = new Producto();

        Console.Write("Código del producto: ");
        nuevo.Codigo = Console.ReadLine();

        Console.Write("Nombre del producto: ");
        nuevo.Nombre = Console.ReadLine();

        Console.Write("Cantidad en stock: ");
        if (!int.TryParse(Console.ReadLine(), out int cantidad))
        {
            Console.WriteLine("Cantidad inválida. Se establecerá a 0.");
            cantidad = 0;
        }
        nuevo.Cantidad = cantidad;

        Console.Write("Precio: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal precio))
        {
            Console.WriteLine("Precio inválido. Se establecerá a 0.");
            precio = 0;
        }
        nuevo.Precio = precio;

        productos[cantidadProductos] = nuevo;
        cantidadProductos++;
        GuardarInventario();

        Console.WriteLine("\nProducto agregado!\n");
    }

    public static void MostrarInventario()
    {
        if (cantidadProductos == 0)
        {
            Console.WriteLine("No hay productos.\n");
            return;
        }

        Console.WriteLine("\nINVENTARIO:");
        Console.WriteLine("Código | Nombre | Cantidad | Precio");
        Console.WriteLine("-----------------------------------");
        for (int i = 0; i < cantidadProductos; i++)
        {
            Console.WriteLine(
                $"{productos[i].Codigo, -6} | {productos[i].Nombre, -15} | {productos[i].Cantidad, 8} | {productos[i].Precio, 10:C}" //Muestra los produtos, el -6 y -15 son para darle formato al texto
            );
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        Inventario.CargarInventario();

        while (true)
        {
            Console.WriteLine("1. Agregar\n2. Mostrar\n3. Salir");
            Console.Write("Opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Inventario.AgregarProducto();
                    break;
                case "2":
                    Inventario.MostrarInventario();
                    break;
                case "3":
                    Inventario.GuardarInventario();
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }
        }
    }
}
