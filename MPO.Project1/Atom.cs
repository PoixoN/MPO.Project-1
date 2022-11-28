using MPO.Project1.Events;

namespace MPO.Project1;

class Atom
{
    private int currentPosition;
    private int N;
    private double p;
    private Random rand;

    public delegate void AtomChangedHandler(object sender, AtomChanged e);
    public event AtomChangedHandler AtomEvent;

    public Atom(int currentIndex, int n, double p)
    {
        currentPosition = currentIndex;
        N = n;
        this.p = p;
        rand = new Random();
    }


    public void RandMove(CancellationToken isStopped)
    {
        while (!isStopped.IsCancellationRequested)
        {
            var probability = rand.NextDouble();

            // move right
            if (probability > p)
            {
                if (currentPosition + 1 < N)
                {
                    currentPosition += 1;
                    AtomEvent(this, new AtomChanged { from = currentPosition - 1, to = currentPosition });
                }
            }

            // move left
            else
            {
                if (currentPosition > 0)
                {
                    currentPosition -= 1;
                    AtomEvent(this, new AtomChanged { from = currentPosition + 1, to = currentPosition });
                }
            }
        }
    }
}
