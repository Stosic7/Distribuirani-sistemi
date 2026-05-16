import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.rmi.server.UnicastRemoteObject;

public class ClientUser {
    static class MatchCallback extends UnicastRemoteObject implements IMatchCallback {
        private IMatch match;

        public MatchCallback(IMatch match) throws RemoteException {
            this.match = match;
        }

        public void resultChanged(int matchId) throws RemoteException {
            System.out.println("PROMENA REZULTATA - " + match.getResult());
        }
    }

    public static void main(String[] args) throws Exception {
        Registry registry = LocateRegistry.getRegistry("localhost", 4096);
        IFootballScore footballScore = (IFootballScore) registry.lookup("FootballScore");

        // prikazi sve utkamice
        System.out.println("=== Sve utakmice ===");
        System.out.println(footballScore.getAllMatches());

        // naziv stadiona za utakmicu sa id 2
        IMatch match2 = footballScore.getMatch(2);
        Stadium stadium = match2.getStadium();
        System.out.println("Stadion za match 2: " + stadium.getName() + ", " + stadium.getCity());

        // pretplati se na dogadjaje utkamice sa id 1
        IMatch match1 = footballScore.getMatch(1);
        MatchCallback callback = new MatchCallback(match1);
        match1.subscribe(callback);
        System.out.println("Pretplacen na match 1. Cekam promene...");

        System.in.read();
    }
}
