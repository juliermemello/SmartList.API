namespace SmartList.Domain.Constants;

public static class DefaultData
{
    public static readonly List<string> Categories = new()
    {
        "Mercearia (Básico)",     // Arroz, feijão, óleo, sal
        "Hortifruti",             // Frutas, legumes, verduras
        "Açougue e Peixaria",     // Carnes, frango, peixes
        "Laticínios e Frios",     // Leite, queijo, iogurte, presunto
        "Padaria",                // Pães, bolos, torradas
        "Congelados",             // Pizzas, hambúrgueres, vegetais congelados
        "Bebidas",                // Sucos, refrigerantes, águas
        "Bebidas Alcoólicas",     // Cervejas, vinhos, destilados
        "Limpeza",                // Desinfetante, sabão em pó, amaciante
        "Higiene Pessoal",        // Shampoo, pasta de dente, sabonete
        "Farmácia e Saúde",       // Remédios, vitaminas, curativos
        "Pet Shop",               // Ração, areia sanitária, petiscos
        "Bebês e Crianças",       // Fraldas, lenços, papinhas
        "Casa e Utensílios",      // Lâmpadas, pilhas, papel toalha
        "Papelaria e Escritório", // Canetas, cadernos, post-its
        "Ferramentas e Jardim"    // Pregos, adubo, reparos rápidos
    };

    public static readonly Dictionary<string, List<string>> Catalog = new()
    {
        { "Mercearia (Básico)", new() { "Arroz Integral (1kg)", "Feijão Carioca (1kg)", "Óleo de Soja (900ml)", "Sal Refinado (1kg)", "Açúcar Refinado (1kg)" } },
        { "Hortifruti", new() { "Banana Prata (Dúzia)", "Alface Crespa", "Tomate Italiano (kg)", "Cebola Branca (kg)", "Maçã Gala (kg)" } },
        { "Açougue e Peixaria", new() { "Alcatra Bovina (kg)", "Peito de Frango (kg)", "Filé de Tilápia (500g)", "Carne Moída (Patinho)", "Costela Suína (kg)" } },
        { "Laticínios e Frios", new() { "Leite Integral (1L)", "Queijo Muçarela (Fatiado)", "Iogurte Natural", "Presunto Cozido", "Manteiga com Sal" } },
        { "Padaria", new() { "Pão Francês (Unidade)", "Pão de Forma Tradicional", "Bolo de Cenoura", "Bisnaguinha (Pacote)", "Croissant de Queijo" } },
        { "Congelados", new() { "Pizza de Calabresa", "Hambúrguer Bovino (Caixa)", "Batata Palito (Pacote)", "Ervilha Congelada", "Lasanha à Bolonhesa" } },
        { "Bebidas", new() { "Suco de Uva Integral (1L)", "Refrigerante de Cola (2L)", "Água Mineral Sem Gás (500ml)", "Chá Gelado de Limão", "Água de Coco (1L)" } },
        { "Bebidas Alcoólicas", new() { "Cerveja Pilsen (Lata)", "Vinho Tinto Seco", "Vodka Premium", "Gin (750ml)", "Espumante Brut" } },
        { "Limpeza", new() { "Detergente Líquido", "Sabão em Pó (1kg)", "Amaciante de Roupas", "Desinfetante de Pinho", "Água Sanitária (1L)" } },
        { "Higiene Pessoal", new() { "Shampoo (400ml)", "Condicionador", "Creme Dental", "Sabonete em Barra", "Desodorante Aerosol" } },
        { "Farmácia e Saúde", new() { "Paracetamol (500mg)", "Vitamina C (Efervescente)", "Curativo Adesivo (Caixa)", "Algodão em Disco", "Álcool 70% (500ml)" } },
        { "Pet Shop", new() { "Ração Seca para Cães (1kg)", "Ração Úmida para Gatos (Sachê)", "Areia Sanitária (4kg)", "Petisco para Cão", "Shampoo Pet Neutralizador" } },
        { "Bebês e Crianças", new() { "Fralda Descartável (Pacote G)", "Lenço Umedecido", "Papinha de Fruta", "Leite em Pó Infantil", "Pomada para Assaduras" } },
        { "Casa e Utensílios", new() { "Lâmpada LED 9W", "Pilha AA (Alcalina)", "Papel Toalha (Rolo)", "Filtro de Café (Nº 103)", "Guardanapo de Papel" } },
        { "Papelaria e Escritório", new() { "Caneta Esferográfica Azul", "Caderno Universitário (10 Matérias)", "Bloco de Notas Adesivas", "Clips de Papel (Caixa)", "Papel A4 (500 folhas)" } },
        { "Ferramentas e Jardim", new() { "Kit de Pregos Variados", "Adubo Orgânico (1kg)", "Fita Isolante (20m)", "Mangueira de Jardim", "Chave de Fenda (Fenda/Philips)" } }
    };
}
