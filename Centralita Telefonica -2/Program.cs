using System;

namespace PracticaCentralita
{
    // 1. ABSTRACCIÓN Y ENCAPSULAMIENTO
    // Pongo esta clase como abstracta porque así aplico el pilar de abstracción. 
    // Representa una llamada genérica, no se puede instanciar directamente.
    public abstract class Llamada
    {
        // Encapsulamiento: Uso protected para que las clases hijas puedan usar estas variables,
        // pero desde fuera (como en el Main) no se puedan tocar directamente.
        protected string numOrigen;
        protected string numDestino;
        protected double duracion;

        // Constructor para inicializar los datos básicos de cualquier llamada
        public Llamada(string numOrigen, string numDestino, double duracion)
        {
            this.numOrigen = numOrigen;
            this.numDestino = numDestino;
            this.duracion = duracion;
        }

        // Getters por si necesito sacar los datos después
        public string GetNumOrigen() => numOrigen;
        public string GetNumDestino() => numDestino;
        public double GetDuracion() => duracion;

        // Método abstracto: obliga a que las clases hijas tengan que decir cómo se calcula el precio.
        public abstract double CalcularPrecio();
    }

    // 2. HERENCIA
    // LlamadaLocal hereda de la clase base Llamada.
    public class LlamadaLocal : Llamada
    {
        private double precio;

        // En el constructor uso "base" para pasarle los parámetros a la clase padre
        public LlamadaLocal(string numOrigen, string numDestino, double duracion)
            : base(numOrigen, numDestino, duracion)
        {
            this.precio = 0.15; // Según el PDF, la local cuesta 15 céntimos el segundo
        }

        // 3. POLIMORFISMO
        // Sobrescribo (override) el método del padre para darle el comportamiento específico de la local
        public override double CalcularPrecio()
        {
            return duracion * precio;
        }

        // También sobrescribo el ToString para que se imprima la info organizada en la consola
        public override string ToString()
        {
            return $"[Local] De: {numOrigen} a {numDestino} | {duracion}s | Total: {CalcularPrecio()} euros";
        }
    }

    // 2. HERENCIA
    // Esta también hereda de Llamada, pero tiene lógica diferente por lo de las franjas.
    public class LlamadaProvincial : Llamada
    {
        // Atributos extra para los precios de este tipo de llamada
        private double precio1 = 0.20;
        private double precio2 = 0.25;
        private double precio3 = 0.30;
        private int franja;

        public LlamadaProvincial(string numOrigen, string numDestino, double duracion, int franja)
            : base(numOrigen, numDestino, duracion)
        {
            this.franja = franja; // Guardo la franja horaria
        }

        // 3. POLIMORFISMO
        public override double CalcularPrecio()
        {
            double precioActual = 0;

            // Valido en qué franja se hizo la llamada para cobrarle según dice el enunciado
            switch (franja)
            {
                case 1: precioActual = precio1; break;
                case 2: precioActual = precio2; break;
                case 3: precioActual = precio3; break;
                default: precioActual = 0; break; // Por si se mete una franja que no existe
            }

            return duracion * precioActual;
        }

        public override string ToString()
        {
            return $"[Provincial] De: {numOrigen} a {numDestino} | {duracion}s | Franja: {franja} | Total: {CalcularPrecio()} euros";
        }
    }

    // Clase que maneja el registro de todo
    public class Centralita
    {
        // Variables para ir sumando las llamadas y el dinero
        private int cont;
        private double acum;

        public Centralita()
        {
            // Arrancamos en cero
            this.cont = 0;
            this.acum = 0.0;
        }

        public int GetTotalLlamadas() => cont;
        public double GetTotalFacturado() => acum;

        // Método para registrar cualquier tipo de llamada (sea Local o Provincial)
        public void RegistrarLlamada(Llamada llamada)
        {
            cont++; // Sumo una llamada más al contador
            acum += llamada.CalcularPrecio(); // Polimorfismo en acción: el sistema sabe qué método usar dependiendo de si es local o provincial

            // El enunciado pide que se muestre por pantalla según se van registrando
            Console.WriteLine($"Registrada: {llamada.ToString()}");
        }
    }

    // Clase principal que pedía el enunciado
    public class Practica2
    {
        public static void Main(string[] args)
        {
            // Instancio mi centralita
            Centralita miCentralita = new Centralita();

            // Me invento algunas llamadas de prueba
            LlamadaLocal ll1 = new LlamadaLocal("8091112222", "8093334444", 45.0);
            LlamadaProvincial ll2 = new LlamadaProvincial("8095556666", "8297778888", 120.0, 1);
            LlamadaProvincial ll3 = new LlamadaProvincial("8099990000", "8491234567", 60.0, 3);

            Console.WriteLine("--- EMPEZANDO REGISTRO DE LA CENTRALITA ---");

            // Registro las llamadas
            miCentralita.RegistrarLlamada(ll1);
            miCentralita.RegistrarLlamada(ll2);
            miCentralita.RegistrarLlamada(ll3);

            // Imprimo el informe final con los contadores
            Console.WriteLine("--- REPORTE FINAL ---");
            Console.WriteLine($"Total de llamadas hechas: {miCentralita.GetTotalLlamadas()}");
            Console.WriteLine($"Total facturado (euros): {miCentralita.GetTotalFacturado():0.00}");

            // Pongo esto para que la consola de Visual Studio no se cierre de golpe al terminar
            Console.ReadLine();
        }
    }
}