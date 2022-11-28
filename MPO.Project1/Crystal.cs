using MPO.Project1.Events;

namespace MPO.Project1;

class Crystal
{
    private readonly int N;
    private readonly int K;
    private readonly double p;
    private readonly bool hasLock;
    private readonly List<int> _cells;

    private object _locker = new object();

    public List<int> Cells => _cells;

    public Crystal(int n, int k, double p, bool withLock)
    {
        N = n;
        K = k;
        this.p = p;
        hasLock = withLock;
        _cells = new List<int>(new int[n]);
        _cells[0] = k;
    }

    public void Start(CancellationToken isStopped)
    {
        for (int i = 0; i < K; i++)
        {
            var atom = new Atom(0, N, p);
            atom.AtomEvent += OnAtomChanged;
            var thread = new Thread(() => atom.RandMove(isStopped));
            thread.Start();
        }
    }

    private void OnAtomChanged(object sender, AtomChanged e)
    {
        if (hasLock)
            MoveWithLock(e);
        else
            MoveWithoutLock(e);
    }

    private void MoveWithLock(AtomChanged e)
    {
        lock (_locker)
        {
            _cells[e.from] -= 1;
            _cells[e.to] += 1;
        }
    }

    private void MoveWithoutLock(AtomChanged e)
    {
        _cells[e.from] -= 1;
        _cells[e.to] += 1;
    }
}
