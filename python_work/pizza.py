def make_pizza(size, *toppings):
    print('\nMaking a ' + size + ' pizza with the following topping:')
    for topping in toppings:
        print('-' + topping)
    