using System;
using System.IO;

class Producto
{
    public string Codigo;
    public string Nombre;
    public int Cantidad;
    public decimal Precio;
}

class Inventario
{
    private const int MaxProductos = 50; //Cantidad máxima de productos
    private static Producto[] productos = new Producto[MaxProductos]; //Arreglo de productos que sirve para guardarlos
    private static int cantidadProductos = 0; //Cantidad de productos en el inventario
    private const string ArchivoInventario = "inventario.csv"; //El nombre del archivo

    public static void CargarInventario() //Carga el inventario
    {
        if (File.Exists(ArchivoInventario))
        {
            string[] lineas = File.ReadAllLines(ArchivoInventario); //File.ReadAllLines lee todas las líneas del archivo
            foreach (string linea in lineas) //Recorre las líneas
            {
                string[] datos = linea.Split(','); //Separa los datos por una coma
                if (datos.Length == 4) //Si hay 4 datos
                {
                    productos[cantidadProductos] = new Producto //Guarda los datos en el arreglo de productos
                    {
                        Codigo = datos[0], //Guarda los datos
                        Nombre = datos[1],
                        Cantidad = int.Parse(datos[2]), //Convierte el dato a entero
                        Precio = decimal.Parse(datos[3]), //Convierte el dato a decimal
                    };
                    cantidadProductos++; //Aumenta la cantidad de productos
                }
            }
        }
    }

    public static void GuardarInventario()
    {
        string[] lineas = new string[cantidadProductos]; //Crea un strings para guardarlos
        for (int i = 0; i < cantidadProductos; i++) //Recorre los productos
        {
            lineas[i] =
                $"{productos[i].Codigo},{productos[i].Nombre},{productos[i].Cantidad},{productos[i].Precio}";
        }
        File.WriteAllLines(ArchivoInventario, lineas); //File.WriteAllLines sirve para escribir en el archivo
    }

    public static void AgregarProducto()
    {
        if (cantidadProductos >= MaxProductos) //Si la cantidad es mayor o igual a la cantidad máxima se sale
        {
            Console.WriteLine("Inventario lleno. No se pueden agregar más productos.\n"); //Mostramos el mensaje
            return;
        }

        Producto nuevo = new Producto();

        Console.Write("Código del producto: ");
        nuevo.Codigo = Console.ReadLine();

        Console.Write("Nombre del producto: ");
        nuevo.Nombre = Console.ReadLine();

        Console.Write("Cantidad en stock: ");
        int.TryParse(Console.ReadLine(), out nuevo.Cantidad);

        Console.Write("Precio: ");
        decimal.TryParse(Console.ReadLine(), out nuevo.Precio);

        productos[cantidadProductos] = nuevo;
        cantidadProductos++;
        GuardarInventario();

        Console.WriteLine("\nProducto agregado\n");
    }

    public static void MostrarInventario()
    {
        if (cantidadProductos == 0)
        {
            Console.WriteLine("No hay productos.\n");
            return;
        }

        Console.WriteLine("\nINVENTARIO:");
        for (int i = 0; i < cantidadProductos; i++) //Recorre los productos
        {
            Console.WriteLine(
                $"{productos[i].Codigo} | {productos[i].Nombre} | {productos[i].Cantidad} | ${productos[i].Precio}" //Muestra los productos
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
                    return;
            }
        }
    }
}
