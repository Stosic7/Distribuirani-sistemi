import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IPrimeService extends Remote {
    void findPrimes(int n, int m, IPrimeCallback callback) throws RemoteException;
}
