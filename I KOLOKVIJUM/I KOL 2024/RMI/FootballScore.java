import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.HashMap;
import java.util.Map;

public class FootballScore extends UnicastRemoteObject implements IFootballScore {
    private Map<Integer, IMatch> matches = new HashMap<>();

    // kreirati dva match objekta sa id 1 i 2
    public FootballScore() throws RemoteException {
        Match match1 = new Match(1, "Crvena zvezda", "Partizan",
                new Stadium("Rajko Mitic", "Beograd"));
        Match match2 = new Match(2, "Barcelona", "Real Madrid",
                new Stadium("Camp Nou", "Barcelona"));

        matches.put(1, match1);
        matches.put(2, match2);
    }

    // vraca string sa svim utakmicama
    public String getAllMatches() throws RemoteException {
        StringBuilder sb = new StringBuilder();
        for (IMatch match : matches.values()) {
            sb.append(match.getResult()).append("\n");
        }
        return sb.toString();
    }

    // vraca jednu utkamicu
    public IMatch getMatch(int id) throws RemoteException {
        return matches.get(id);
    }
}
