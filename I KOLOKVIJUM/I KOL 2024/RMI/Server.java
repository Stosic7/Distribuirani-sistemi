import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class Server {
    public static void main(String[] args) throws Exception {
        FootballScore footballScore = new FootballScore();
        Registry registry = LocateRegistry.createRegistry(4096);
        registry.rebind("FootballScore", footballScore);
        System.out.println("Server pokrenut na portu 4096.");
    }
}
