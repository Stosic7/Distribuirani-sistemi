import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IFootballScore extends Remote {
    String getAllMatches() throws RemoteException;
    IMatch getMatch(int id) throws RemoteException;
}
