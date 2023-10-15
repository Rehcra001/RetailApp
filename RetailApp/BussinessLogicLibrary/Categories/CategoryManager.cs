using ModelsLibrary;
using ModelsLibrary.RepositoryInterfaces;

namespace BussinessLogicLibrary.Categories
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Gets a list of all product categories
        /// </summary>
        /// <returns>
        /// Returns an IEnumerable<CategoryModel>
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the categories
        /// </exception>
        public IEnumerable<CategoryModel> GetAll()
        {
            Tuple<IEnumerable<CategoryModel>, string> categories = _categoryRepository.GetAll().ToTuple();
            //Check for errors
            if (categories.Item2 == null)
            {
                //No Errors
                return categories.Item1;
            }
            else
            {
                //Error raised
                throw new Exception(categories.Item2);
            }
        }

        /// <summary>
        /// Gets a category by ID
        /// </summary>
        /// <param name="id">
        /// Takes in an integer for ID
        /// </param>
        /// <returns>
        /// Returns a CategoryModel
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if unable to retrieve the category
        /// </exception>
        public CategoryModel GetByID(int id)
        {
            Tuple<CategoryModel, string> category = _categoryRepository.GetByID(id).ToTuple();
            //Check for Errors
            if (category.Item2 == null)
            {
                //No Error
                return category.Item1;
            }
            else
            {
                //error raised
                throw new Exception(category.Item2);
            }
        }

        /// <summary>
        /// Inserts a new category
        /// </summary>
        /// <param name="category">
        /// Takes in a CategoryModel
        /// </param>
        /// <returns>
        /// Returns a CategoryModel with ID
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if any validation errors encountered. 
        /// Throws an exception if unable to insert the new category
        /// </exception>
        public CategoryModel Insert(CategoryModel category)
        {
            //check if any validation errors raised
            if (category.Validate())
            {
                //No errors
                Tuple<CategoryModel, string> insertedCategory = _categoryRepository.Insert(category).ToTuple();
                //check for errors
                if (insertedCategory.Item2 == null)
                {
                    //No errors
                    return insertedCategory.Item1;
                }
                else
                {
                    //Error raised
                    throw new Exception(insertedCategory.Item2);
                }
            }
            else
            {
                //Error validating
                throw new Exception(category.ValidationMessage);
            }
        }

        /// <summary>
        /// Updates an existing category
        /// </summary>
        /// <param name="category">
        /// Takes in a CategoryModel
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception if any validation errors encountered. 
        /// Throws an exception if unable to update the category
        /// </exception>
        public void Update(CategoryModel category)
        {
            //check if any validation errors raised
            if (category.Validate())
            {
                //No validation errors
                string errorMessage = _categoryRepository.Update(category);
                //check for errors
                if (errorMessage != null)
                {
                    //Error raised
                    throw new Exception(errorMessage);
                }
            }
            else
            {
                //Error validating
                throw new Exception(category.ValidationMessage);
            }
        }
    }
}
