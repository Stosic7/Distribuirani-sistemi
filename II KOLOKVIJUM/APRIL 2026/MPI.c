#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>

#define W MPI_COMM_WORLD
#define FILESIZE (1024 * 1024)
#define INPUT "datoteke/input.dat"
#define OUTPUT "datoteke/april.dat"

int main(int argc, char** argv) {
    int rank, size;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(W, &rank);
    MPI_Comm_size(W, &size);

    MPI_File fh;
    MPI_Status status;

    // svaki process obradjuje jednak deo fajla
    int bufsize = FILESIZE / size; // koliko svaki proces ima brojeva u bajtovima
    int nints = bufsize / sizeof(int); // koliko intova ima svaki proces
    int half = nints / 2;

    int* buf = (int*)malloc(bufsize);

    // citanje, reverse, individualni pokazivac

    MPI_Offset offset = (MPI_Offset)(size - 1 - rank) * bufsize;

    MPI_File_open(W, INPUT, MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);
    MPI_File_seek(fh, offset, MPI_SEEK_SET); // individualni pokazivac
    MPI_File_read(fh, buf, nints, MPI_INT, &status);
    MPI_File_close(&fh);

    printf("Proces %d procitao deo sa offsetom %lld: prvi=%d poslednji=%d\n", rank, (long long)offset, buf[0], buf[nints - 1]);
	fflush(stdout);

    // upis, dve polovine u dve zove, grupna operacija
    // ilustrovana ideja:
    // ako P0 ima intove: 1,2,3,4,5,6,7,8
    // nints = 8, half = 4, rank = 0. size = 4 (pretpostavka da imamo 4 procesa)
    // blocklenghts = {half, half} to je zato sto nam treba prva polovina da ide u prvi deo fajla druga u drugu polovinu fajla
    // displacements (koliko da preskoci nakon svakog upisi) =  {rank * half, size * half + rank * half}, prva polovina zavisi samo od ranka
    //                                                                                                    znaci to se upisuje linearno
    //                                                                                                    druga polovina mora da prekosici CEO PRVI DEO (size * half, 4 procesa pa puta prva polovina koja je upisana)
    //                                                                                                    plus offset od ranka koji ima process
    // znaci za P0, bice ovakav: blocklengths = {4, 4}, displacements = {0, 16}, znaci napisace na pozicije 0,1,2,3 prvu polovinu (1,2,3,4) a onda ce krene od (16,17,18,19) i tu ce da upise drugu polovinu (5,6,7,8)
    // mozda ima bolji nacin ali ovaj mi je prvi pao na pamet
    int blocklengths[2] = {half, half}; 
    int displacements[2] = {rank * half, size * half + rank * half};

    MPI_Datatype mojtip;
    MPI_Type_indexed(2, blocklengths, displacements, MPI_INT, &mojtip);
    MPI_Type_commit(&mojtip);

    MPI_File_open(W, OUTPUT, MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);
    MPI_File_set_view(fh, 0, MPI_INT, mojtip, "native", MPI_INFO_NULL);
    MPI_File_write_all(fh, buf, nints, MPI_INT, &status);
    MPI_File_close(&fh);

    MPI_Type_free(&mojtip);
    free(buf);

    if (rank == 0) {
		printf("\n------------------------------------------------\n");
		printf("Gotovo. Procitano reverzno iz input.dat, upisano po polovinama u april.dat.\n");
		fflush(stdout);
	}

    MPI_Finalize();
    return 0;
}
