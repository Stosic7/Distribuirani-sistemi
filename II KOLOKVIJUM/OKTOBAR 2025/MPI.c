#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>

#define W MPI_COMM_WORLD
#define FILESIZE (10 * 1024 * 1024)
#define INPUT "datoteke/inputokt22025.dat"
#define OUTPUT "datoteke/okt2output.dat"

int main(int argc, char** argv) {
	int rank, size;
	MPI_Init(&argc, &argv);
	MPI_Comm_rank(W, &rank);
	MPI_Comm_size(W, &size);

	MPI_File fh;
	MPI_Status status;

	// svaki process obradjuje jednak deo fajla
	// koristimo osmobitni tip (MPI_BYTE) => 1 element = 1 bajt, pa je broj elemenata bas bufsize
	int bufsize = FILESIZE / size;
	int fourths = bufsize / 4;

	unsigned char* buf = (unsigned char*)malloc(bufsize);

	// citanje
	MPI_Offset offset = (MPI_Offset)(size - 1 - rank) * bufsize;
	MPI_File_open(W, INPUT, MPI_MODE_RDONLY, MPI_INFO_NULL, &fh);
	MPI_File_seek(fh, offset, MPI_SEEK_SET);
	MPI_File_read(fh, buf, bufsize, MPI_BYTE, &status);
	MPI_File_close(&fh);

	printf("Proces %d procitao deo sa offsetom %lld: prvi=%d poslednji=%d\n",
	       rank, (long long)offset, buf[0], buf[bufsize - 1]);
	fflush(stdout);

	// upis
	int blocklen[4] = { fourths, fourths, fourths, fourths };
	int displ[4] = {
		rank * fourths,
		size * fourths + rank * fourths,
		2 * size * fourths + rank * fourths,
		3 * size * fourths + rank * fourths
	};

	MPI_Datatype mojtip;
	MPI_Type_indexed(4, blocklen, displ, MPI_BYTE, &mojtip);
	MPI_Type_commit(&mojtip);

	MPI_File_open(W, OUTPUT, MPI_MODE_CREATE | MPI_MODE_WRONLY, MPI_INFO_NULL, &fh);
	MPI_File_set_view(fh, 0, MPI_BYTE, mojtip, "native", MPI_INFO_NULL);
	MPI_File_write_all(fh, buf, bufsize, MPI_BYTE, &status);
	MPI_File_close(&fh);

	MPI_Type_free(&mojtip);
	free(buf);

	if (rank == 0) {
		printf("\n------------------------------------------------\n");
		printf("Gotovo. Procitano reverzno iz inputokt22025.dat, upisano po cetvrtinama u okt2output.dat.\n");
		fflush(stdout);
	}

	MPI_Finalize();
	return 0;
}
