import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ISubscribeCallback extends Remote {
    void onMessage(String topic, Message message) throws RemoteException;
}
