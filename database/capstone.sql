USE master
GO

--drop database if it exists
IF DB_ID('final_capstone') IS NOT NULL
BEGIN
	ALTER DATABASE final_capstone SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE final_capstone;
END

CREATE DATABASE final_capstone
GO

USE final_capstone
GO

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL
	CONSTRAINT PK_user PRIMARY KEY (user_id)
)
CREATE TABLE ingredients (
	ingredients_id int IDENTITY (1,1) NOT NULL, 
	ingredients_name varchar(30) NOT NULL
	CONSTRAINT PK_ingredients PRIMARY KEY (ingredients_id)
)

CREATE TABLE recipes (
	recipe_id int IDENTITY (1,1) NOT NULL,
	recipe_name varchar(30) NOT NULL,
	description varchar(750) NOT NULL,
	instructions varchar(750) NOT NULL
	CONSTRAINT PK_recipes PRIMARY KEY (recipe_id) 
)
CREATE TABLE recipe_ingredients (
	recipe_id int NOT NULL,
	ingredients_id int NOT NULL,
	CONSTRAINT PK_recipe_ingredients PRIMARY KEY (recipe_id, ingredients_id),
	CONSTRAINT FK_recipe_ingredients_recipes FOREIGN KEY (recipe_id) REFERENCES recipes(recipe_id),
	CONSTRAINT FK_recipe_ingredients_ingredients FOREIGN KEY (ingredients_id) REFERENCES ingredients(ingredients_id)
)

CREATE TABLE accounts (
	account_id int IDENTITY(1,1) NOT NULL,
	user_id int NOT NULL, 
	recipe_id int NOT NULL
	CONSTRAINT PK_accounts PRIMARY KEY (account_id),
	CONSTRAINT FK_user FOREIGN KEY (user_id) REFERENCES users(user_id),
	CONSTRAINT FK_recipes FOREIGN KEY (recipe_id) REFERENCES recipes(recipe_id)
)

CREATE TABLE meals (
	meal_id int IDENTITY(1,1) NOT NULL,
	meal_date date NOT NULL, 
	meal_name varchar(30) NOT NULL, 
	meal_type varchar(30) NOT NULL
	CONSTRAINT PK_meals PRIMARY KEY (meal_id)
	
	
)
CREATE TABLE meal_recipe (
	meal_id int NOT NULL, 
	recipe_id int NOT NULL
	CONSTRAINT PK_meal_recipe PRIMARY KEY (recipe_id, meal_id),
	CONSTRAINT FK_meal_recipe_meals FOREIGN KEY (meal_id) REFERENCES meals(meal_id),
	CONSTRAINT FK_meal_reciep_recipes FOREIGN KEY (recipe_id) REFERENCES recipes(recipe_id)
)



--populate default data
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');
INSERT INTO users (username, password_hash, salt, user_role) VALUES ('admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin');
INSERT INTO ingredients (ingredients_name) VALUES ('');
INSERT INTO ingredients (ingredients_name) VALUES ('Bread');
INSERT INTO ingredients (ingredients_name) VALUES ('Grape Jelly');
INSERT INTO ingredients (ingredients_name) VALUES ('Peanut Butter');
INSERT INTO ingredients (ingredients_name) VALUES ('Ham');
INSERT INTO ingredients (ingredients_name) VALUES ('Cheese');
INSERT INTO ingredients (ingredients_name) VALUES ('Wrap');
INSERT INTO ingredients (ingredients_name) VALUES ('Apples');
INSERT INTO ingredients (ingredients_name) VALUES ('Blueberries');
INSERT INTO ingredients (ingredients_name) VALUES ('Strawberries');
INSERT INTO ingredients (ingredients_name) VALUES ('Oranges');
INSERT INTO ingredients (ingredients_name) VALUES ('Bananas');
INSERT INTO ingredients (ingredients_name) VALUES ('Cinnamon');
INSERT INTO ingredients (ingredients_name) VALUES ('Egg');
INSERT INTO ingredients (ingredients_name) VALUES ('Shredded Cheese');
INSERT INTO ingredients (ingredients_name) VALUES ('Tortilla');
INSERT INTO ingredients (ingredients_name) VALUES ('Noodles');
INSERT INTO ingredients (ingredients_name) VALUES ('Water');
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('','','')
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('Peanut Butter & Jelly Sandwich', 'Rich smooth peanut butter with grape jelly on white bread', '1. Use two slices of bread. 2. Spread Peanut Butter on one slice. 3. Spread Jelly on other slice. 4. Combine slices of bread, slice diagonally if preferred.');
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('Ham & Cheese Wrap', 'Ham & Cheese in wrap, always a nice easy lunch', '1. Lay wrap flat. 2. Place two slices of ham on wrap. 3. Place one slice of cheese on wrap 4. Roll wrap and slice in the center to split in two');
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('Fruit Salad', 'Variety of bite size fruits mixed together', '1. Combine apples, blueberries, strawberries, and oranges together.  2. Mix in a bowl and enjoy.');
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('Banana Pancakes', 'Healthy banana pancakes', '1. Mash one banana, egg, cinnamon together.  2.Put pancake mixture into a pan and cook pancake. 3.Serve pancakes');
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('Quesadillas', 'Cheesy Quesadilla', '1.Put shredded cheese on a tortilla.  2.Put more shredded cheese on tortilla. 3.Fold tortilla in half. 4.Cook and serve Quesadilla');
INSERT INTO recipes (recipe_name, description, instructions) VALUES ('Ramen Noodles', 'Noodles and a beef based broth, too much sodium. Be careful!', '1. Bring 2 1/2 cups of water to a boil in a small saucepan 2. Add the noodles and cook for 2 minutes 3. Add the flavor packet, stir, and continue to cook for another 30 seconds 4. Remove the pan from the heat and carefully add an egg');
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (2, 2);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (2, 3);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (3, 4);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (3, 5);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (3, 6);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (3, 7);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (4, 8);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (4, 9);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (4, 10);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (4, 11);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (5, 12);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (5, 13);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (5, 14);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (6, 15);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (6, 16);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (7, 17);
INSERT INTO recipe_ingredients (recipe_id, ingredients_id) VALUES (7, 18);
INSERT INTO accounts (user_id, recipe_id) VALUES (1, 1);
INSERT INTO meals (meal_date, meal_name, meal_type) VALUES ('08-11-2020','PB & Jelly Sandwhich', 'Lunch')
INSERT INTO meals (meal_date, meal_name, meal_type) VALUES ('08-11-2020','Fruit Salad', 'Brunch')
INSERT INTO meals (meal_date, meal_name, meal_type) VALUES ('08-11-2020','Banana Pancakes', 'Breakfast')
INSERT INTO meals (meal_date, meal_name, meal_type) VALUES ('08-11-2020','Ramen', 'Dinner')
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (1, 2)
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (2, 4)
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (3, 5)
INSERT INTO meal_recipe (meal_id, recipe_id) VALUES (4, 7)

GO
