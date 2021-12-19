#include <iostream>
#include <mkl.h>
#include <mkl_df.h>
#include <mkl_df_types.h>

using namespace std;

#define SPLINE_ORDER DF_PP_CUBIC /* A cubic spline to construct */

/* Parameters describing the partition */
// MKL_INT nx;          /* The size of partition x */
// double x[NX];         /* Partition x */
/* Parameters describing the function */
// MKL_INT ny;          /* Function dimension */
// double y[NX];         /* Function values at the breakpoints */
// double r[ny * nx * n];   /* Array of interpolation results */
int makeSpline(MKL_INT nx, MKL_INT ny, double x[2], double* y, int n, double* r)
{
    int status;     /* Status of a Data Fitting operation */
    DFTaskPtr task; /* Data Fitting operations are task based */

    {
        MKL_INT xhint; /* Additional information about the structure of breakpoints */
        MKL_INT yhint; /* Additional information about the function */
        /* Set values of partition x */
        xhint = DF_UNIFORM_PARTITION; /* The partition is non-uniform. */
        /* Set function values */
        yhint = DF_MATRIX_STORAGE_ROWS; /* No additional information about the function is provided. */

        /* Create a Data Fitting task */
        status = dfdNewTask1D(&task, nx, x, xhint, ny, y, yhint);
        /* Check the Data Fitting operation status */
        if (status != DF_STATUS_OK)
        {
            return status;
        }
    }

    {
        /* Parameters describing the spline */
        MKL_INT s_order; /* Spline order */
        MKL_INT s_type;  /* Spline type */
        MKL_INT ic_type; /* Type of internal conditions */
        double* ic;      /* Array of internal conditions */
        MKL_INT bc_type; /* Type of boundary conditions */
        double* bc;      /* Array of boundary conditions */

        double* scoeff = new double[(nx - 1) * SPLINE_ORDER]; /* Array of spline coefficients */
        MKL_INT scoeffhint;                     /* Additional information about the coefficients */

        /* Initialize spline parameters */
        s_order = DF_PP_CUBIC;  /* Spline is of the fourth order (cubic spline). */
        s_type = DF_PP_NATURAL; /* Spline is of the Natural cubic type. */
        /* Define internal conditions for cubic spline construction */
        ic_type = DF_NO_IC;
        ic = NULL;

        /* Use not-a-knot boundary conditions. In this case, the is first and the last
        interior breakpoints are inactive, no additional values are provided. */
        bc_type = DF_BC_NOT_A_KNOT;
        bc = NULL;
        scoeffhint = DF_NO_HINT; /* No additional information about the spline. */

        /* Set spline parameters  in the Data Fitting task */
        status = dfdEditPPSpline1D(task, s_order, s_type, bc_type, bc, ic_type,
            ic, scoeff, scoeffhint);
        /* Check the Data Fitting operation status */
        if (status != DF_STATUS_OK)
        {
            return status;
        }
    }

    status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    /* Check the Data Fitting operation status */
    if (status != DF_STATUS_OK)
    {
        return status;
    }

    {
        /* Parameters describing interpolation computations */
        MKL_INT nsite;           /* Number of interpolation sites */
        double site[2];          /* Array of interpolation sites */
        MKL_INT sitehint;        /* Additional information about the structure of
                                interpolation sites */
        MKL_INT ndorder, dorder; /* Parameters defining the type of interpolation */
        double* datahint;        /* Additional information on partition and interpolation sites */
        //double r[ny * nx * n];   /* Array of interpolation results */
        MKL_INT rhint; /* Additional information on the structure of the results */
        MKL_INT* cell; /* Array of cell indices */
        /* Initialize interpolation parameters */
        nsite = n * nx;

        /* Set site values */
        site[0] = x[0];
        site[1] = x[1];

        sitehint = DF_UNIFORM_PARTITION; /* Partition of sites is uniform */

        /* Request to compute spline values */
        ndorder = 1;
        dorder = 1;
        datahint = DF_NO_APRIORI_INFO;  /* No additional information about breakpoints or
                                        sites is provided. */
        rhint = DF_MATRIX_STORAGE_ROWS; /* The library packs interpolation results
                                        in row-major format. */
        cell = NULL;                    /* Cell indices are not required. */

        /* Solve interpolation problem using the default method: compute the spline values
        at the points site(i), i=0,..., nsite-1 and place the results to array r */
        status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site,
            sitehint, ndorder, &dorder, datahint, r, rhint, cell);
        if (status != DF_STATUS_OK)
        {
            return status;
        }
    }

    /* De-allocate Data Fitting task resources */
    status = dfDeleteTask(&task);
    /* Check Data Fitting operation status */
    if (status != DF_STATUS_OK)
    {
        return status;
    }
    return 0;
}

void makeSplines(double** data, int nx, int ny, int stepx, int n, double* res)
{
    double x[2] = { 0, (nx - 1) * stepx };
    double* y = new double[nx * ny];
    for (int i = 0; i < nx; ++i)
        for (int j = 0; j < ny; ++j)
            y[i * nx + j] = data[i][j];

    int status = makeSpline(nx, ny, x, y, n, res);
}