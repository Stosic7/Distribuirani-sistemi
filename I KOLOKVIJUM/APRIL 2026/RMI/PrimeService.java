import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;

public class PrimeService extends UnicastRemoteObject implements IPrimeService {
    public PrimeService() throws RemoteException { }

    public void findPrimes(int n, int m, IPrimeCallback callback) throws RemoteException {
        System.out.println("Trazim proste brojeve izmedju " + n + " i " + m);
        for (int i = n; i <= m; i++) {
            if (isPrime(i)) {
                System.out.println("Pronadjen prost broj: " + i);
                callback.onPrime(i);
            }
        }
    }

    public boolean isPrime(int num) {
        if (num < 2) return false;
        for (int i = 2; i <= Math.sqrt(num); i++) {
            if (num % i == 0) return false;
        }
        return true;
    }
}
