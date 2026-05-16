import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IMatch extends Remote{
    void addHomeGoal() throws RemoteException;
    void addAwayGoal() throws RemoteException;
    Stadium getStadium() throws RemoteException;
    String getResult() throws RemoteException;
    void subscribe(IMatchCallback cb) throws RemoteException;
    void unsubscribe(IMatchCallback cb) throws RemoteException;
}
