import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class Server {
    public static void main(String[] args) throws Exception {
        PrimeService primeService = new PrimeService();

        Registry registry = LocateRegistry.createRegistry(1099);
        registry.rebind("PrimeService", primeService);

        System.out.println("Prime server pokrenut.");
    }
}
