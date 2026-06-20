#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>

#define W MPI_COMM_WORLD
#define BROJ 105

#define FILE1 "datoteke/file1.dat"
#define FILE2 "datoteke/file2.dat"

int main(int argc, char** argv) {
    int rank, size;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(W, &rank);
    MPI_Comm_size(W, &size);

    MPI_File fh;
    MPI_Status status;

    // bafer sa proizvoljnim brojevima
    int buf[BROJ];
    for (int i = 0; i < BROJ; i++) {
        buf[i] = rank * 1000 + i;
    }

    // a) upis u file1.dat -> individualni pokazivac
    // redosled u fajlu: poslednji process prvi, prvi process poslednji

    // sto znaci da svaki process ima offset (size - 1 - rank) * broj * sizeof(int)

    MPI_Offset offset = (MPI_Offset)(size-1-rank) * BROJ * sizeof(int);
    MPI_File_open(W, FILE1, MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);

    // individualni pokazivac = seek pa write
    MPI_File_seek(fh, offset, MPI_SEEK_SET);
    printf("Process [%d], pisem sa offsetom [%lld]\n", rank, offset);
    fflush(stdout);
    MPI_File_write(fh, buf, BROJ, MPI_INT, &status);
    printf("Process [%d], upisao.\n", rank);
    fflush(stdout);
    
    MPI_File_close(&fh);
    MPI_Barrier(W);
    printf("gotovo pod a\n");
    fflush(stdout);

    // b) citanje upravo upisanih podataka, eksplicitni pomeraj
    // svaki process cita tacno ono sto je upisao

    int readbuf[BROJ];

    MPI_File_open(W, FILE1, MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);
    MPI_File_read_at(fh, offset, readbuf, BROJ, MPI_INT, &status); // ovo je eksplicitni poeraj, read_at
    MPI_File_close(&fh);

    // provera
    printf("Proces %d procitao sa offsetom [%lld]\n", rank, offset);
	fflush(stdout);
    MPI_Barrier(W);
    printf("Zavrseno pod b\n");
    fflush(stdout);

    // c) upis u file2.dat
    // mora MPI_Type_indexed, jer blokovi rastu
    // svaki process upisuje k elemenata

    printf("Krecemo pod c\n");
    fflush(stdout);

    int rounds = 0, acc = 0;
    while (acc < BROJ) {
        rounds++;
        acc += rounds;
    }

    int *blcoklengths = (int*)malloc(rounds * sizeof(int));
    int *displacements = (int*)malloc(rounds * sizeof(int));

    for (int k = 1; k <= rounds; k++) {
        int pocetak_runde = size * (k - 1) * k / 2;
        blcoklengths[k - 1] = k;
        displacements[k - 1] = pocetak_runde + rank * k; 
    }

    MPI_Datatype mojtip;
    MPI_Type_indexed(rounds, blcoklengths, displacements, MPI_INT, &mojtip);
    MPI_Type_commit(&mojtip);

    MPI_File_open(W, FILE2, MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);
    MPI_File_set_view(fh, 0, MPI_INT, mojtip, "native", MPI_INFO_NULL);
    MPI_File_write_all(fh, readbuf, BROJ, MPI_INT, &status);
    MPI_File_close(&fh);

    MPI_Type_free(&mojtip);
    free(blcoklengths);
    free(displacements);

    if (rank == 0) {
		printf("\n------------------------------------------------\n");
		printf("Gotovo. file1.dat (reverzni) i file2.dat su upisani.\n");
		fflush(stdout);
	}

    MPI_Finalize();
    return 0;
}
