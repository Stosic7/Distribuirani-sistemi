import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IMatchCallback extends Remote {
    void resultChanged(int matchId) throws RemoteException; 
}
