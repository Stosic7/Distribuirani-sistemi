import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.rmi.server.UnicastRemoteObject;

public class Client {
    static class PrimeCallback extends UnicastRemoteObject implements IPrimeCallback {
        public PrimeCallback() throws RemoteException { }

        public void onPrime(int prime) throws RemoteException {
            System.out.println("Primljena povratna informacija: " + prime);
        }
    }

    public static void main(String[] args) throws Exception {
        Registry registry = LocateRegistry.getRegistry("localhost", 1099);
        IPrimeService primeService = (IPrimeService) registry.lookup("PrimeService");

        PrimeCallback callback = new PrimeCallback();
        int n = 10;
        int m = 50;
        primeService.findPrimes(n, m, callback);
        System.out.println("Trazim proste brojeve izmedju " + n + " i " + m);
        
        primeService.findPrimes(n, m, callback);
        System.out.println("Zahtev poslan.");

    }
}
