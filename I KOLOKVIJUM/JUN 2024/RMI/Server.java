import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class Server {
    public static void main(String[] args) throws Exception {
        MQTTBroker broker = new MQTTBroker();
        Registry registry = LocateRegistry.createRegistry(1099);
        registry.rebind("MQTTBroker", broker);
        System.out.println("MQTT Broker je pokrenut...");
    }
}
