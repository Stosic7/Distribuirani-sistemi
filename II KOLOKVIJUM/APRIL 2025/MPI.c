#include <stdio.h>
#include <stdlib.h>
#include <mpi.h>

#define n 6
#define k 6
#define s 2
#define W MPI_COMM_WORLD

int main(int argc, char** argv) {
    int rank, size;

    MPI_Init(&argc, &argv);
    MPI_Comm_rank(W, &rank);
    MPI_Comm_size(W, &size);

    int A[n][k];
    int B[k];
    int local[n][s];

    if (rank == 0) {
		printf("Inicijalna matrica A:\n");
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < k; j++) {
				A[i][j] = (i + j) % 10;
				printf("%d\t", A[i][j]);
			}
			printf("\n");
		}
		fflush(stdout);
		printf("------------------------------------------------\n");

        int max = A[0][0];
        int remI;
        int remJ;
        for (int i = 0; i < n; i++) {
			for (int j = 0; j < k; j++) {
				if (A[i][j] > max) {
                    max = A[i][j];
                    remI = i;
                    remJ = j;
                }
			}
		}

        printf("Maksimalna vrednost u matrici A: %d na poziciji A[%d][%d]\n", max, remI, remJ);
        printf("------------------------------------------------\n");

        int sume[n];
        for (int i = 0; i < n; i++) {
            int suma = 0;
			for (int j = 0; j < k; j++) {
				suma += A[i][j];
			}
            printf("suma vrste %d = %d\n", i, suma);
            sume[i] = max;
		}
        printf("------------------------------------------------\n");

		printf("Inicijalna vector B: ");
		for (int i = 0; i < k; i++) {
			B[i] = i + 1;
			printf("%d ", B[i]);
		}
		printf("\n");
		printf("------------------------------------------------\n");
	}


    MPI_Datatype col_type;
    MPI_Type_vector(n, s, k, MPI_INT, &col_type);
    MPI_Type_commit(&col_type);

    // raspodela kolona A: POINT-TO-POINT
    if (rank == 0) {
        for (int p = 1; p < size; p++)
            MPI_Send(&A[0][p*s], 1, col_type, p, 0, W);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < s; j++)
                local[i][j] = A[i][j];
    } else {
        MPI_Recv(local, n*s, MPI_INT, 0, 0, W, MPI_STATUS_IGNORE);
    }

    // raspodela b: grupna, po s elemenata
    int b_local[s];
    MPI_Scatter(B, s, MPI_INT, b_local, s, MPI_INT, 0, W);

    int partial[n];
    for (int i = 0; i < n; i++) {
        partial[i] = 0;
        for (int j = 0; j < s; j++)
            partial[i] += local[i][j] * b_local[j];
    }

    // print za proveru parcijalnog mnozenja
    printf("Proces %d -> kolone %d,%d | b_local = ", rank, rank*s, rank*s+1);
    for (int j = 0; j < s; j++) printf("%d ", b_local[j]);
    printf("\n");
    for (int i = 0; i < n; i++) {
        printf("   P%d red %d: ", rank, i);
        for (int j = 0; j < s; j++) printf("%d ", local[i][j]);
        printf("  -> partial[%d] = %d\n", i, partial[i]);
    }
    fflush(stdout);

    MPI_Finalize();
    return 0;
}
