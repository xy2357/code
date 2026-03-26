def factorial(n):
    fac = 1
    if n == 0 or n == 1:
        return fac
    else:
        for i in range(1, n + 1):
            fac *= i
        return fac
print(factorial(100))