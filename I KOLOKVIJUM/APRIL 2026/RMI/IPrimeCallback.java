import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IPrimeCallback extends Remote {
    void onPrime(int prime) throws RemoteException;
}
