// Admin Dashboard JavaScript

// Sample data for frontend functionality
let products = [
    { id: 1, name: "Casual Blue Shirt", description: "Comfortable cotton shirt for casual wear", category: "Men's Clothing", categoryId: 1, price: 49.99, isAvailable: true, imageUrl: "https://via.placeholder.com/60" },
    { id: 2, name: "Summer Dress", description: "Light floral dress perfect for summer", category: "Women's Clothing", categoryId: 2, price: 59.99, isAvailable: true, imageUrl: "https://via.placeholder.com/60" },
    { id: 3, name: "Leather Shoes", description: "Premium leather formal shoes", category: "Footwear", categoryId: 3, price: 79.99, isAvailable: false, imageUrl: "https://via.placeholder.com/60" },
    { id: 4, name: "Winter Jacket", description: "Warm winter jacket with hood", category: "Outerwear", categoryId: 5, price: 129.99, isAvailable: true, imageUrl: "https://via.placeholder.com/60" },
    { id: 5, name: "Casual Sneakers", description: "Comfortable sneakers for everyday wear", category: "Footwear", categoryId: 3, price: 69.99, isAvailable: true, imageUrl: "https://via.placeholder.com/60" }
];

let categories = [
    { id: 1, name: "Men's Clothing", productsCount: 24 },
    { id: 2, name: "Women's Clothing", productsCount: 36 },
    { id: 3, name: "Footwear", productsCount: 18 },
    { id: 4, name: "Accessories", productsCount: 29 },
    { id: 5, name: "Outerwear", productsCount: 15 }
];

let orders = [
    { id: "#ORD-1234", customer: "Ahmed Mohamed", products: 3, total: 120.00, date: "May 3, 2025", status: "Completed" },
    { id: "#ORD-1233", customer: "Noha Ahmed", products: 1, total: 85.00, date: "May 3, 2025", status: "Processing" },
    { id: "#ORD-1232", customer: "Mohamed Ali", products: 5, total: 240.00, date: "May 2, 2025", status: "Completed" },
    { id: "#ORD-1231", customer: "Sara Hassan", products: 2, total: 95.00, date: "May 2, 2025", status: "Shipped" },
    { id: "#ORD-1230", customer: "Khaled Ahmed", products: 4, total: 185.00, date: "May 1, 2025", status: "Completed" }
];

let customers = [
    { id: 1, name: "Ahmed Mohamed", email: "ahmed@example.com", orders: 8, totalSpent: 450.00, joined: "Jan 15, 2025" },
    { id: 2, name: "Noha Ahmed", email: "noha@example.com", orders: 5, totalSpent: 320.00, joined: "Feb 3, 2025" },
    { id: 3, name: "Mohamed Ali", email: "mohamed@example.com", orders: 12, totalSpent: 780.00, joined: "Dec 10, 2024" },
    { id: 4, name: "Sara Hassan", email: "sara@example.com", orders: 3, totalSpent: 150.00, joined: "Mar 22, 2025" },
    { id: 5, name: "Khaled Ahmed", email: "khaled@example.com", orders: 7, totalSpent: 520.00, joined: "Jan 5, 2025" }
];

// DOM ready
document.addEventListener('DOMContentLoaded', function() {
    // Initialize event listeners
    initEventListeners();
    
    // Initialize modals using Bootstrap
    var modals = document.querySelectorAll('.modal');
    Array.from(modals).forEach(modal => {
        new bootstrap.Modal(modal);
    });
});

function initEventListeners() {
    // Product related actions
    document.getElementById('addProductBtn')?.addEventListener('click', showAddProductModal);
    document.querySelectorAll('.edit-product-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const productId = this.getAttribute('data-id');
            showEditProductModal(productId);
        });
    });
    document.querySelectorAll('.delete-product-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const productId = this.getAttribute('data-id');
            showDeleteProductConfirmation(productId);
        });
    });
    
    // Category related actions
    document.getElementById('addCategoryBtn')?.addEventListener('click', showAddCategoryModal);
    document.querySelectorAll('.edit-category-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const categoryId = this.getAttribute('data-id');
            showEditCategoryModal(categoryId);
        });
    });
    document.querySelectorAll('.delete-category-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const categoryId = this.getAttribute('data-id');
            showDeleteCategoryConfirmation(categoryId);
        });
    });
    
    // Form submissions
    document.getElementById('addProductForm')?.addEventListener('submit', handleAddProduct);
    document.getElementById('editProductForm')?.addEventListener('submit', handleEditProduct);
    document.getElementById('addCategoryForm')?.addEventListener('submit', handleAddCategory);
    document.getElementById('editCategoryForm')?.addEventListener('submit', handleEditCategory);
    
    // Settings form
    document.getElementById('settingsForm')?.addEventListener('submit', handleSettingsSave);
}

// Product Functions
function showAddProductModal() {
    // Clear form
    document.getElementById('addProductForm').reset();
    
    // Populate categories dropdown
    const categorySelect = document.getElementById('productCategory');
    categorySelect.innerHTML = '';
    categories.forEach(cat => {
        const option = document.createElement('option');
        option.value = cat.id;
        option.textContent = cat.name;
        categorySelect.appendChild(option);
    });
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('addProductModal'));
    modal.show();
}

function handleAddProduct(event) {
    event.preventDefault();
    
    // In a real app, this would send data to server
    // Here we'll just update our frontend data
    
    const form = event.target;
    const newId = products.length > 0 ? Math.max(...products.map(p => p.id)) + 1 : 1;
    const categoryId = parseInt(form.productCategory.value);
    const category = categories.find(c => c.id === categoryId)?.name || '';
    
    const newProduct = {
        id: newId,
        name: form.productName.value,
        description: form.productDescription.value,
        price: parseFloat(form.productPrice.value),
        isAvailable: form.productAvailability.checked,
        categoryId: categoryId,
        category: category,
        imageUrl: "https://via.placeholder.com/60" // Default image for demo
    };
    
    // Add to local array
    products.push(newProduct);
    
    // Update UI
    const tbody = document.querySelector('#products table tbody');
    const row = document.createElement('tr');
    row.innerHTML = `
        <td><img src="${newProduct.imageUrl}" class="product-image-thumbnail" alt="Product"></td>
        <td>${newProduct.id}</td>
        <td>${newProduct.name}</td>
        <td>${newProduct.category}</td>
        <td>$${newProduct.price.toFixed(2)}</td>
        <td><span class="badge ${newProduct.isAvailable ? 'bg-success' : 'bg-secondary'} badge-status">
            ${newProduct.isAvailable ? 'Available' : 'Out of Stock'}
        </span></td>
        <td class="actions">
            <button class="btn btn-sm btn-primary edit-product-btn" data-id="${newProduct.id}"><i class="fas fa-edit"></i></button>
            <button class="btn btn-sm btn-danger delete-product-btn" data-id="${newProduct.id}"><i class="fas fa-trash"></i></button>
        </td>
    `;
    tbody.appendChild(row);
    
    // Add event listeners to new buttons
    row.querySelector('.edit-product-btn').addEventListener('click', function() {
        showEditProductModal(newProduct.id);
    });
    row.querySelector('.delete-product-btn').addEventListener('click', function() {
        showDeleteProductConfirmation(newProduct.id);
    });
    
    // Update products count for the category
    const categoryIndex = categories.findIndex(c => c.id === categoryId);
    if (categoryIndex !== -1) {
        categories[categoryIndex].productsCount++;
        const categoryRow = document.querySelector(`#categories table tbody tr:nth-child(${categoryIndex + 1}) td:nth-child(3)`);
        if (categoryRow) {
            categoryRow.textContent = categories[categoryIndex].productsCount;
        }
    }
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('addProductModal'));
    modal.hide();
    
    // Show success message
    showToast('Product added successfully!');
}

function showEditProductModal(productId) {
    const product = products.find(p => p.id == productId);
    if (!product) return;
    
    // Populate form
    const form = document.getElementById('editProductForm');
    form.productId.value = product.id;
    form.productName.value = product.name;
    form.productDescription.value = product.description;
    form.productPrice.value = product.price;
    form.productAvailability.checked = product.isAvailable;
    
    // Populate categories dropdown
    const categorySelect = document.getElementById('editProductCategory');
    categorySelect.innerHTML = '';
    categories.forEach(cat => {
        const option = document.createElement('option');
        option.value = cat.id;
        option.textContent = cat.name;
        if (cat.id === product.categoryId) {
            option.selected = true;
        }
        categorySelect.appendChild(option);
    });
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('editProductModal'));
    modal.show();
}

function handleEditProduct(event) {
    event.preventDefault();
    
    const form = event.target;
    const productId = parseInt(form.productId.value);
    const productIndex = products.findIndex(p => p.id === productId);
    
    if (productIndex === -1) return;
    
    const oldCategoryId = products[productIndex].categoryId;
    const newCategoryId = parseInt(form.editProductCategory.value);
    const newCategory = categories.find(c => c.id === newCategoryId)?.name || '';
    
    // Update product in array
    products[productIndex] = {
        ...products[productIndex],
        name: form.productName.value,
        description: form.productDescription.value,
        price: parseFloat(form.productPrice.value),
        isAvailable: form.productAvailability.checked,
        categoryId: newCategoryId,
        category: newCategory
    };
    
    // Update UI
    const productRow = document.querySelector(`#products table tbody tr:nth-child(${productIndex + 1})`);
    if (productRow) {
        productRow.innerHTML = `
            <td><img src="${products[productIndex].imageUrl}" class="product-image-thumbnail" alt="Product"></td>
            <td>${products[productIndex].id}</td>
            <td>${products[productIndex].name}</td>
            <td>${products[productIndex].category}</td>
            <td>$${products[productIndex].price.toFixed(2)}</td>
            <td><span class="badge ${products[productIndex].isAvailable ? 'bg-success' : 'bg-secondary'} badge-status">
                ${products[productIndex].isAvailable ? 'Available' : 'Out of Stock'}
            </span></td>
            <td class="actions">
                <button class="btn btn-sm btn-primary edit-product-btn" data-id="${products[productIndex].id}"><i class="fas fa-edit"></i></button>
                <button class="btn btn-sm btn-danger delete-product-btn" data-id="${products[productIndex].id}"><i class="fas fa-trash"></i></button>
            </td>
        `;
        
        // Re-add event listeners
        productRow.querySelector('.edit-product-btn').addEventListener('click', function() {
            showEditProductModal(products[productIndex].id);
        });
        productRow.querySelector('.delete-product-btn').addEventListener('click', function() {
            showDeleteProductConfirmation(products[productIndex].id);
        });
    }
    
    // Update category counts if changed
    if (oldCategoryId !== newCategoryId) {
        // Decrease count for old category
        const oldCategoryIndex = categories.findIndex(c => c.id === oldCategoryId);
        if (oldCategoryIndex !== -1) {
            categories[oldCategoryIndex].productsCount--;
            const oldCategoryRow = document.querySelector(`#categories table tbody tr:nth-child(${oldCategoryIndex + 1}) td:nth-child(3)`);
            if (oldCategoryRow) {
                oldCategoryRow.textContent = categories[oldCategoryIndex].productsCount;
            }
        }
        
        // Increase count for new category
        const newCategoryIndex = categories.findIndex(c => c.id === newCategoryId);
        if (newCategoryIndex !== -1) {
            categories[newCategoryIndex].productsCount++;
            const newCategoryRow = document.querySelector(`#categories table tbody tr:nth-child(${newCategoryIndex + 1}) td:nth-child(3)`);
            if (newCategoryRow) {
                newCategoryRow.textContent = categories[newCategoryIndex].productsCount;
            }
        }
    }
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('editProductModal'));
    modal.hide();
    
    // Show success message
    showToast('Product updated successfully!');
}

function showDeleteProductConfirmation(productId) {
    const product = products.find(p => p.id == productId);
    if (!product) return;
    
    // Set product ID and name in the modal
    document.getElementById('deleteProductId').value = product.id;
    document.getElementById('deleteProductName').textContent = product.name;
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('deleteProductModal'));
    modal.show();
    
    // Set up delete confirmation button
    document.getElementById('confirmDeleteProduct').onclick = function() {
        deleteProduct(productId);
    };
}

function deleteProduct(productId) {
    const productIndex = products.findIndex(p => p.id == productId);
    if (productIndex === -1) return;
    
    // Store category ID for updating count
    const categoryId = products[productIndex].categoryId;
    
    // Remove from array
    products.splice(productIndex, 1);
    
    // Remove from UI
    const productRow = document.querySelector(`#products table tbody tr:nth-child(${productIndex + 1})`);
    if (productRow) {
        productRow.remove();
    }
    
    // Update category count
    const categoryIndex = categories.findIndex(c => c.id === categoryId);
    if (categoryIndex !== -1) {
        categories[categoryIndex].productsCount--;
        const categoryRow = document.querySelector(`#categories table tbody tr:nth-child(${categoryIndex + 1}) td:nth-child(3)`);
        if (categoryRow) {
            categoryRow.textContent = categories[categoryIndex].productsCount;
        }
    }
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('deleteProductModal'));
    modal.hide();
    
    // Show success message
    showToast('Product deleted successfully!');
}

// Category Functions
function showAddCategoryModal() {
    // Clear form
    document.getElementById('addCategoryForm').reset();
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('addCategoryModal'));
    modal.show();
}

function handleAddCategory(event) {
    event.preventDefault();
    
    const form = event.target;
    const newId = categories.length > 0 ? Math.max(...categories.map(c => c.id)) + 1 : 1;
    
    const newCategory = {
        id: newId,
        name: form.categoryName.value,
        productsCount: 0
    };
    
    // Add to local array
    categories.push(newCategory);
    
    // Update UI
    const tbody = document.querySelector('#categories table tbody');
    const row = document.createElement('tr');
    row.innerHTML = `
        <td>${newCategory.id}</td>
        <td>${newCategory.name}</td>
        <td>${newCategory.productsCount}</td>
        <td class="actions">
            <button class="btn btn-sm btn-primary edit-category-btn" data-id="${newCategory.id}"><i class="fas fa-edit"></i></button>
            <button class="btn btn-sm btn-danger delete-category-btn" data-id="${newCategory.id}"><i class="fas fa-trash"></i></button>
        </td>
    `;
    tbody.appendChild(row);
    
    // Add event listeners to new buttons
    row.querySelector('.edit-category-btn').addEventListener('click', function() {
        showEditCategoryModal(newCategory.id);
    });
    row.querySelector('.delete-category-btn').addEventListener('click', function() {
        showDeleteCategoryConfirmation(newCategory.id);
    });
    
    // Update product category dropdown
    const categoryOption = document.createElement('option');
    categoryOption.value = newCategory.id;
    categoryOption.textContent = newCategory.name;
    document.getElementById('productCategory')?.appendChild(categoryOption);
    document.getElementById('editProductCategory')?.appendChild(categoryOption.cloneNode(true));
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('addCategoryModal'));
    modal.hide();
    
    // Show success message
    showToast('Category added successfully!');
}

function showEditCategoryModal(categoryId) {
    const category = categories.find(c => c.id == categoryId);
    if (!category) return;
    
    // Populate form
    const form = document.getElementById('editCategoryForm');
    form.categoryId.value = category.id;
    form.categoryName.value = category.name;
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('editCategoryModal'));
    modal.show();
}

function handleEditCategory(event) {
    event.preventDefault();
    
    const form = event.target;
    const categoryId = parseInt(form.categoryId.value);
    const categoryIndex = categories.findIndex(c => c.id === categoryId);
    
    if (categoryIndex === -1) return;
    
    const oldName = categories[categoryIndex].name;
    const newName = form.categoryName.value;
    
    // Update category in array
    categories[categoryIndex].name = newName;
    
    // Update UI
    const categoryRow = document.querySelector(`#categories table tbody tr:nth-child(${categoryIndex + 1})`);
    if (categoryRow) {
        categoryRow.querySelector('td:nth-child(2)').textContent = newName;
    }
    
    // Update all products with this category
    products.forEach((product, idx) => {
        if (product.categoryId === categoryId) {
            products[idx].category = newName;
            
            // Update product UI
            const productRow = document.querySelector(`#products table tbody tr:nth-child(${idx + 1})`);
            if (productRow) {
                productRow.querySelector('td:nth-child(4)').textContent = newName;
            }
        }
    });
    
    // Update dropdown options
    document.querySelectorAll('#productCategory option, #editProductCategory option').forEach(option => {
        if (option.value == categoryId) {
            option.textContent = newName;
        }
    });
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('editCategoryModal'));
    modal.hide();
    
    // Show success message
    showToast('Category updated successfully!');
}

function showDeleteCategoryConfirmation(categoryId) {
    const category = categories.find(c => c.id == categoryId);
    if (!category) return;
    
    // Set category ID and name in the modal
    document.getElementById('deleteCategoryId').value = category.id;
    document.getElementById('deleteCategoryName').textContent = category.name;
    
    // Show warning if products exist in this category
    const hasProducts = products.some(p => p.categoryId == categoryId);
    document.getElementById('categoryDeleteWarning').style.display = hasProducts ? 'block' : 'none';
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('deleteCategoryModal'));
    modal.show();
    
    // Set up delete confirmation button
    document.getElementById('confirmDeleteCategory').onclick = function() {
        deleteCategory(categoryId);
    };
}

function deleteCategory(categoryId) {
    const categoryIndex = categories.findIndex(c => c.id == categoryId);
    if (categoryIndex === -1) return;
    
    // Remove from array
    categories.splice(categoryIndex, 1);
    
    // Remove from UI
    const categoryRow = document.querySelector(`#categories table tbody tr:nth-child(${categoryIndex + 1})`);
    if (categoryRow) {
        categoryRow.remove();
    }
    
    // Update or remove products in this category
    products.forEach((product, idx) => {
        if (product.categoryId == categoryId) {
            // Either remove these products or set them to "Uncategorized"
            // For demo, we'll just update to "Uncategorized"
            products[idx].category = "Uncategorized";
            products[idx].categoryId = 0;
            
            // Update product UI
            const productRow = document.querySelector(`#products table tbody tr:nth-child(${idx + 1})`);
            if (productRow) {
                productRow.querySelector('td:nth-child(4)').textContent = "Uncategorized";
            }
        }
    });
    
    // Remove from dropdowns
    document.querySelectorAll('#productCategory option, #editProductCategory option').forEach(option => {
        if (option.value == categoryId) {
            option.remove();
        }
    });
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('deleteCategoryModal'));
    modal.hide();
    
    // Show success message
    showToast('Category deleted successfully!');
}

// Settings Functions
function handleSettingsSave(event) {
    event.preventDefault();
    
    // In a real app, this would save to server
    // Here we'll just show a success message
    
    // Close any open collapse
    const collapseEls = document.querySelectorAll('#settings .collapse.show');
    collapseEls.forEach(collapse => {
        bootstrap.Collapse.getInstance(collapse).hide();
    });
    
    // Show success message
    showToast('Settings saved successfully!');
}

// Utility function for showing toast messages
function showToast(message) {
    const toastContainer = document.getElementById('toastContainer');
    
    // Create toast if it doesn't exist
    if (!toastContainer) {
        const container = document.createElement('div');
        container.id = 'toastContainer';
        container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        document.body.appendChild(container);
    }
    
    const toastId = 'toast-' + Date.now();
    const toastHTML = `
        <div id="${toastId}" class="toast align-items-center text-white bg-success border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    `;
    
    document.getElementById('toastContainer').insertAdjacentHTML('beforeend', toastHTML);
    
    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement, { delay: 3000 });
    toast.show();
    
    // Remove toast after it's hidden
    toastElement.addEventListener('hidden.bs.toast', function() {
        toastElement.remove();
    });
}
