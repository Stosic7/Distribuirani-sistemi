import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class MQTTBroker extends UnicastRemoteObject implements IMQTTBroker {
    private Map<String, List<ISubscribeCallback>> topics = new HashMap<>();
    public MQTTBroker() throws RemoteException {}

    private void createTopic(String topic) {
        if (!topics.containsKey(topic)) {
            topics.put(topic, new ArrayList<>());
        }
    }

    // klijent se pretplacuje na topic
    public void subscribe(String topic, ISubscribeCallback callback) throws RemoteException {
        createTopic(topic);
        topics.get(topic).add(callback);
        System.out.println("Novi pretplatnik na topik: " + topic);
    }

    // publisher salje poruku na topic
    public void publish(String topic, Message message) throws RemoteException {
        createTopic(topic);
        System.out.println("Objavljena poruka na topiku: " + topic + " - " + message);
        for (ISubscribeCallback callback : topics.get(topic)) {
            callback.onMessage(topic, message);
        }
    }
}
