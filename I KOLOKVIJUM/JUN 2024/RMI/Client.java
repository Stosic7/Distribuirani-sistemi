import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.rmi.server.UnicastRemoteObject;

public class Client {
    static class MyCallback extends UnicastRemoteObject implements ISubscribeCallback {
         
        public MyCallback() throws RemoteException { }

        public void onMessage(String topic, Message message) throws RemoteException {
            System.out.println("Primljena poruka na topiku: " + topic + " - " + message);
        }
    }

    public static void main(String[] args) throws Exception {
        Registry registry = LocateRegistry.getRegistry("localhost", 1099);
        IMQTTBroker broker = (IMQTTBroker) registry.lookup("MQTTBroker");

        MyCallback callback1 = new MyCallback();
        broker.subscribe("sport", callback1);
        System.out.println("Pretplacen na topik: sport");

        MyCallback callback2 = new MyCallback();
        broker.subscribe("muzika", callback2);
        System.out.println("Pretplacen na topik: muzika");

        broker.publish("sport", new Message("Finale", "Srbija pobedila!"));
        broker.publish("muzika", new Message("Koncert", "AC/DC dolazi u Beograd!"));
        broker.publish("tech", new Message("AI", "ChatGPT nova verzija!"));
    }
}
