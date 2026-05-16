import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class ClientAdmin {
    public static void main(String[] args) throws Exception {
        Registry registry = LocateRegistry.getRegistry("localhost", 4096);
        IFootballScore footballScore = (IFootballScore) registry.lookup("FootballScore");

        // dodaj gol domacinu utakmice sa id 1
        IMatch match1 = footballScore.getMatch(1);
        match1.addHomeGoal();

        System.out.println("Dodat gol domacinu u match 1.");
        System.out.println("Trenutni rezultat: " + match1.getResult());
    }
}
