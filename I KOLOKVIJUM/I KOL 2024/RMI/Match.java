import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;
import java.util.List;


public class Match extends UnicastRemoteObject implements IMatch{
    private int id;
    private String homeTeam;
    private String awayTeam;
    private int homeGoals;
    private int awayGoals;
    private Stadium stadium;

    private List<IMatchCallback> subscribers = new ArrayList<>();

    public Match(int id, String homeTeam, String awayTeam, Stadium stadium)
            throws RemoteException {
        this.id = id;
        this.homeTeam = homeTeam;
        this.awayTeam = awayTeam;
        this.homeGoals = 0;
        this.awayGoals = 0;
        this.stadium = stadium;
    }

    public void addHomeGoal() throws RemoteException {
        homeGoals++;
        notifySubscribers();
    }

    public void addAwayGoal() throws RemoteException {
        awayGoals++;
        notifySubscribers();
    }

    public Stadium getStadium() throws RemoteException {
        return stadium;
    }

    public String getResult() throws RemoteException {
        return "Match " + id + ": " + homeTeam + " " +
               homeGoals + " - " + awayGoals + " " + awayTeam;
    }

    public void subscribe(IMatchCallback cb) throws RemoteException {
        subscribers.add(cb);
        System.out.println("Klijent se pretplatio na match " + id);
    }

    public void unsubscribe(IMatchCallback cb) throws RemoteException {
        subscribers.remove(cb);
    }

    private void notifySubscribers() throws RemoteException {
        for (IMatchCallback cb : subscribers) {
            cb.resultChanged(id);
        }
    }
    
}
