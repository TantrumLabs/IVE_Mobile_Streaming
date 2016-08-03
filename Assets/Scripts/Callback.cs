public delegate void Callback();                                    // No Parameters 
public delegate void Callback<T>(T arg1);                           // 1 Generic Parameter
public delegate void Callback<T, U>(T arg1, U arg2);                // 2 Generic Parameters
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);     // 3 Generic Parameters
