import java.rmi.Remote;
import java.rmi.RemoteException;


public interface IMQTTBroker extends Remote {
    void subscribe(String topic, ISubscribeCallback callback) throws RemoteException;
    void publish(String topic, Message message) throws RemoteException;
}
