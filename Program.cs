using System;
using System.Threading;

class Filosofo
{
    private int id;
    private Semaphore[] tenedores;
    private Random random;

    public Filosofo(int id, Semaphore[] tenedores)
    {
        this.id = id;
        this.tenedores = tenedores;
        this.random = new Random();
    }

    public void Comer()
    {
        while (true)
        {
            Pensar();
            TomarTenedores();
            Masticar();
            SoltarTenedores();
        }
    }

    private void Pensar()
    {
        Console.WriteLine($"Filósofo {id} está pensando.");
        Thread.Sleep(random.Next(1000, 3000));
    }

    private void TomarTenedores()
    {
        if (id % 2 == 0)
        {
            tenedores[id].WaitOne(); // Tomar tenedor izquierdo
            tenedores[(id + 1) % 5].WaitOne(); // Tomar tenedor derecho
        }
        else
        {
            tenedores[(id + 1) % 5].WaitOne(); // Tomar tenedor derecho
            tenedores[id].WaitOne(); // Tomar tenedor izquierdo
        }
        Console.WriteLine($"Filósofo {id} ha tomado los tenedores y está comiendo.");
    }

    private void Masticar()
    {
        Console.WriteLine($"Filósofo {id} está masticando.");
        Thread.Sleep(random.Next(1000, 2000));
    }

    private void SoltarTenedores()
    {
        tenedores[id].Release(); // Soltar tenedor izquierdo
        tenedores[(id + 1) % 5].Release(); // Soltar tenedor derecho
        Console.WriteLine($"Filósofo {id} ha soltado los tenedores.");
    }
}

class Programa
{
    static void Main()
    {
        Semaphore[] tenedores = new Semaphore[5];
        Filosofo[] filosofos = new Filosofo[5];
        Thread[] hilos = new Thread[5];

        for (int i = 0; i < 5; i++)
        {
            tenedores[i] = new Semaphore(1, 1); // Inicializar semáforos para cada tenedor
        }

        for (int i = 0; i < 5; i++)
        {
            filosofos[i] = new Filosofo(i, tenedores);
            hilos[i] = new Thread(new ThreadStart(filosofos[i].Comer));
            hilos[i].Start();
        }

        for (int i = 0; i < 5; i++)
        {
            hilos[i].Join(); // Esperar a que terminen los hilos (aunque en este caso, nunca terminarán)
        }
    }
}
