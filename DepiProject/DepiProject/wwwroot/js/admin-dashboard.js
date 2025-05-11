// Admin Dashboard JavaScript

// Sample data for frontend functionality
let products = [
    { id: 1, name: "iPhone 15 Pro", description: "Apple's latest flagship smartphone with A17 Pro chip", category: "Smartphones", categoryId: 1, brand: "Apple", brandId: 1, price: 999.99, stock: 42, isAvailable: true, modelNumber: "A2650", warranty: 12, specs: "A17 Pro chip, 6.1\" OLED display, 48MP camera", imageUrl: "https://via.placeholder.com/60", isFeatured: true },
    { id: 2, name: "MacBook Pro 16\"", description: "Powerful laptop for professionals with M3 Pro chip", category: "Laptops & Computers", categoryId: 2, brand: "Apple", brandId: 1, price: 2499.99, stock: 15, isAvailable: true, modelNumber: "A2780", warranty: 12, specs: "M3 Pro chip, 16\" Liquid Retina XDR display, 32GB RAM, 1TB SSD", imageUrl: "https://via.placeholder.com/60", isFeatured: true },
    { id: 3, name: "Sony WH-1000XM5", description: "Premium noise cancelling headphones", category: "Audio & Headphones", categoryId: 3, brand: "Sony", brandId: 3, price: 349.99, stock: 0, isAvailable: false, modelNumber: "WH1000XM5", warranty: 24, specs: "Industry-leading noise cancellation, 30-hour battery life, LDAC support", imageUrl: "https://via.placeholder.com/60", isFeatured: false },
    { id: 4, name: "Samsung 65\" OLED 4K TV", description: "Premium OLED TV with stunning picture quality", category: "TV & Home Theater", categoryId: 4, brand: "Samsung", brandId: 2, price: 1799.99, stock: 8, isAvailable: true, modelNumber: "QN65S95C", warranty: 24, specs: "OLED display, 4K resolution, Quantum HDR, Smart TV features", imageUrl: "https://via.placeholder.com/60", isFeatured: true },
    { id: 5, name: "Google Pixel 8 Pro", description: "Google's flagship smartphone with advanced AI features", category: "Smartphones", categoryId: 1, brand: "Google", brandId: 4, price: 899.99, stock: 23, isAvailable: true, modelNumber: "GA03822-US", warranty: 12, specs: "Google Tensor G3, 6.7\" display, 50MP camera system, 512GB storage", imageUrl: "https://via.placeholder.com/60", isFeatured: false }
];

let categories = [
    { id: 1, name: "Smartphones", productsCount: 42, featured: true, icon: "fa-mobile-alt" },
    { id: 2, name: "Laptops & Computers", productsCount: 38, featured: true, icon: "fa-laptop" },
    { id: 3, name: "Audio & Headphones", productsCount: 25, featured: false, icon: "fa-headphones" },
    { id: 4, name: "TV & Home Theater", productsCount: 18, featured: true, icon: "fa-tv" },
    { id: 5, name: "Cameras", productsCount: 15, featured: false, icon: "fa-camera" },
    { id: 6, name: "Gaming", productsCount: 27, featured: true, icon: "fa-gamepad" },
    { id: 7, name: "Smart Home", productsCount: 14, featured: false, icon: "fa-home" },
    { id: 8, name: "Wearable Tech", productsCount: 8, featured: false, icon: "fa-clock" }
];

let orders = [
    { id: "#ORD-5423", customer: "John Smith", customerEmail: "john@example.com", customerPhone: "555-123-4567", products: 3, total: 1249.97, date: "May 10, 2025", status: "Delivered" },
    { id: "#ORD-5422", customer: "Emily Johnson", customerEmail: "emily@example.com", customerPhone: "555-987-6543", products: 1, total: 899.99, date: "May 9, 2025", status: "Processing" },
    { id: "#ORD-5421", customer: "Michael Chen", customerEmail: "michael@example.com", customerPhone: "555-456-7890", products: 4, total: 3849.96, date: "May 8, 2025", status: "Shipped" },
    { id: "#ORD-5420", customer: "Sarah Williams", customerEmail: "sarah@example.com", customerPhone: "555-321-6547", products: 2, total: 1199.98, date: "May 7, 2025", status: "Delivered" },
    { id: "#ORD-5419", customer: "David Thompson", customerEmail: "david@example.com", customerPhone: "555-789-1234", products: 1, total: 2499.99, date: "May 6, 2025", status: "Delivered" }
];

let customers = [
    { id: 1, name: "John Smith", email: "john@example.com", phone: "555-123-4567", orders: 8, totalSpent: 5467.92, joined: "Feb 15, 2024" },
    { id: 2, name: "Emily Johnson", email: "emily@example.com", phone: "555-987-6543", orders: 5, totalSpent: 3245.75, joined: "Mar 2, 2024" },
    { id: 3, name: "Michael Chen", email: "michael@example.com", phone: "555-456-7890", orders: 12, totalSpent: 8912.45, joined: "Nov 18, 2023" },
    { id: 4, name: "Sarah Williams", email: "sarah@example.com", phone: "555-321-6547", orders: 3, totalSpent: 1875.50, joined: "Apr 12, 2024" },
    { id: 5, name: "David Thompson", email: "david@example.com", phone: "555-789-1234", orders: 7, totalSpent: 6325.89, joined: "Jan 22, 2024" }
];

// DOM ready
document.addEventListener('DOMContentLoaded', function() {
    // Initialize event listeners
    initEventListeners();
    
    // Initialize event listeners for the newly added buttons in the recent orders section
    document.querySelectorAll('#dashboard .view-order-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            const orderId = this.getAttribute('data-id');
            showOrderDetails(orderId);
        });
    });
    
    document.querySelectorAll('#dashboard .edit-order-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            const orderId = this.getAttribute('data-id');
            showUpdateOrderStatusModal(orderId);
        });
    });

    // Initialize modals using Bootstrap
    var modals = document.querySelectorAll('.modal');
    Array.from(modals).forEach(modal => {
        new bootstrap.Modal(modal);
    });
    
    // Initialize charts if they exist
    if(document.getElementById('salesChart')) {
        initSalesChart();
    }
    
    if(document.getElementById('categoryChart')) {
        initCategoryChart();
    }
});

function initEventListeners() {
    // Product related actions
    document.getElementById('addProductBtn')?.addEventListener('click', showAddProductModal);
    document.querySelectorAll('.edit-product-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const productId = parseInt(this.getAttribute('data-id'));
            showEditProductModal(productId);
        });
    });
    document.querySelectorAll('.delete-product-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const productId = parseInt(this.getAttribute('data-id'));
            showDeleteProductConfirmation(productId);
        });
    });
    document.querySelectorAll('.view-product-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const productId = parseInt(this.getAttribute('data-id'));
            // Implement view product details functionality
            console.log(`View product with ID: ${productId}`);
        });
    });
    
    // Category related actions
    document.getElementById('addCategoryBtn')?.addEventListener('click', showAddCategoryModal);
    document.querySelectorAll('.edit-category-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const categoryId = parseInt(this.getAttribute('data-id'));
            showEditCategoryModal(categoryId);
        });
    });
    document.querySelectorAll('.delete-category-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const categoryId = parseInt(this.getAttribute('data-id'));
            showDeleteCategoryConfirmation(categoryId);
        });
    });
    
    // Order related actions
    document.querySelectorAll('.view-order-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const orderId = this.getAttribute('data-id');
            showOrderDetails(orderId);
        });
    });
    document.querySelectorAll('.edit-order-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const orderId = this.getAttribute('data-id');
            // Implement edit order functionality
            console.log(`Edit order with ID: ${orderId}`);
        });
    });
    document.querySelectorAll('.cancel-order-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const orderId = this.getAttribute('data-id');
            // Implement cancel order functionality
            console.log(`Cancel order with ID: ${orderId}`);
        });
    });
    
    // Customer related actions
    document.querySelectorAll('.view-customer-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const customerId = parseInt(this.getAttribute('data-id'));
            showCustomerDetails(customerId);
        });
    });
    document.querySelectorAll('.edit-customer-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const customerId = parseInt(this.getAttribute('data-id'));
            showEditCustomerModal(customerId);
        });
    });
    document.querySelectorAll('.delete-customer-btn')?.forEach(btn => {
        btn.addEventListener('click', function() {
            const customerId = parseInt(this.getAttribute('data-id'));
            // Implement delete customer functionality
            console.log(`Delete customer with ID: ${customerId}`);
        });
    });
    
    // Form submissions
    document.getElementById('addProductForm')?.addEventListener('submit', handleAddProduct);
    document.getElementById('editProductForm')?.addEventListener('submit', handleEditProduct);
    document.getElementById('addCategoryForm')?.addEventListener('submit', handleAddCategory);
    document.getElementById('editCategoryForm')?.addEventListener('submit', handleEditCategory);
    document.getElementById('editCustomerBtn')?.addEventListener('click', function() {
        const customerId = parseInt(document.getElementById('customerDetailsId').value);
        showEditCustomerModal(customerId);
    });
      // Modal action buttons
    document.getElementById('confirmDeleteProduct')?.addEventListener('click', handleDeleteProduct);
    document.getElementById('confirmDeleteCategory')?.addEventListener('click', handleDeleteCategory);
    document.getElementById('updateOrderStatus')?.addEventListener('click', function() {
        const orderId = document.getElementById('orderNumber').textContent;
        showUpdateOrderStatusModal(orderId);
    });
    document.getElementById('saveStatusUpdateBtn')?.addEventListener('click', handleOrderStatusUpdate);
    document.getElementById('printInvoiceBtn')?.addEventListener('click', function() {
        const orderId = document.getElementById('orderNumber').textContent;
        handlePrintInvoice(orderId);
    });
    document.getElementById('addCustomerNoteBtn')?.addEventListener('click', handleAddCustomerNote);
    
    // Settings form
    document.getElementById('settingsForm')?.addEventListener('submit', handleSettingsSave);
    
    // Same as shipping checkbox in edit customer form
    document.getElementById('sameAsShipping')?.addEventListener('change', function() {
        const billingFields = document.getElementById('billingAddressFields');
        if (this.checked) {
            billingFields.style.display = 'none';
        } else {
            billingFields.style.display = 'block';
        }
    });
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

// Order Functions
function showOrderDetails(orderId) {
    // Find the order by ID
    const order = orders.find(o => o.id.replace('#ORD-', '') === orderId);
    if (!order) {
        console.error('Order not found:', orderId);
        return;
    }
    
    // Sample data for order details
    const orderDetails = {
        id: order.id,
        customer: {
            name: order.customer,
            email: order.customerEmail,
            phone: order.customerPhone,
            address: '123 Tech Street',
            city: 'San Francisco',
            zip: '94105'
        },
        products: [
            { name: 'iPhone 15 Pro', quantity: 1, price: 999.99, total: 999.99 },
            { name: 'AirPods Pro', quantity: 1, price: 249.99, total: 249.99 }
        ],
        shipping: 0,
        subtotal: order.total - 0, // Assuming free shipping
        total: order.total,
        timeline: [
            { date: '2025-05-08 08:15', status: 'Order Placed', description: 'Order has been placed successfully' },
            { date: '2025-05-08 14:30', status: 'Payment Confirmed', description: 'Payment has been confirmed' },
            { date: '2025-05-09 09:45', status: 'Processing', description: 'Order is being processed' }
        ]
    };
    
    // Add timeline entries based on status
    if (order.status === 'Shipped') {
        orderDetails.timeline.push({ 
            date: '2025-05-10 11:20', 
            status: 'Shipped', 
            description: 'Order has been shipped via Express Delivery' 
        });
    }
    
    if (order.status === 'Delivered') {
        orderDetails.timeline.push({ 
            date: '2025-05-10 11:20', 
            status: 'Shipped', 
            description: 'Order has been shipped via Express Delivery' 
        });
        orderDetails.timeline.push({ 
            date: '2025-05-12 14:15', 
            status: 'Delivered', 
            description: 'Order has been delivered successfully' 
        });
    }
    
    // Populate modal with data
    document.getElementById('orderNumber').textContent = order.id.replace('#ORD-', '');
    document.getElementById('customerName').textContent = order.customer;
    document.getElementById('customerEmail').textContent = order.customerEmail;
    document.getElementById('customerPhone').textContent = order.customerPhone;
    document.getElementById('shippingAddress').textContent = orderDetails.customer.address;
    document.getElementById('shippingCity').textContent = orderDetails.customer.city;
    document.getElementById('shippingZip').textContent = orderDetails.customer.zip;
    
    // Populate products
    const orderItemsContainer = document.getElementById('orderItems');
    orderItemsContainer.innerHTML = '';
    orderDetails.products.forEach(product => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${product.name}</td>
            <td>${product.quantity}</td>
            <td>$${product.price.toFixed(2)}</td>
            <td>$${product.total.toFixed(2)}</td>
        `;
        orderItemsContainer.appendChild(row);
    });
    
    // Set totals
    document.getElementById('subtotal').textContent = `$${orderDetails.subtotal.toFixed(2)}`;
    document.getElementById('shippingCost').textContent = `$${orderDetails.shipping.toFixed(2)}`;
    document.getElementById('total').textContent = `$${orderDetails.total.toFixed(2)}`;
    
    // Populate timeline
    const timelineContainer = document.getElementById('orderTimeline');
    timelineContainer.innerHTML = '';
    orderDetails.timeline.forEach((event, index) => {
        const item = document.createElement('div');
        item.className = 'timeline-item ' + (index === orderDetails.timeline.length - 1 ? 'active' : '');
        item.innerHTML = `
            <div class="timeline-badge"></div>
            <div class="timeline-content">
                <p class="mb-0"><small>${event.date}</small></p>
                <h6 class="fw-bold mb-0">${event.status}</h6>
                <p class="text-muted">${event.description}</p>
            </div>
        `;
        timelineContainer.appendChild(item);
    });
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('orderDetailsModal'));
    modal.show();
}

function showUpdateOrderStatusModal(orderId) {
    const order = orders.find(o => o.id.replace('#ORD-', '') === orderId);
    if (!order) return;
    
    // Populate order status dropdown with current status selected
    const statusSelect = document.getElementById('orderStatusUpdate');
    statusSelect.value = order.status.toLowerCase();
    
    // Store order ID for reference
    document.getElementById('updateOrderId').value = orderId;
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('updateOrderStatusModal'));
    modal.show();
}

function handleOrderStatusUpdate(event) {
    event.preventDefault();
    
    const orderId = document.getElementById('updateOrderId').value;
    const newStatus = document.getElementById('orderStatusUpdate').value;
    
    // Find order index
    const orderIndex = orders.findIndex(o => o.id.replace('#ORD-', '') === orderId);
    if (orderIndex === -1) return;
    
    // Update status
    const statusCapitalized = newStatus.charAt(0).toUpperCase() + newStatus.slice(1);
    orders[orderIndex].status = statusCapitalized;
    
    // Update UI
    const statusBadgeClassMap = {
        'pending': 'bg-secondary',
        'processing': 'bg-warning text-dark',
        'shipped': 'bg-info',
        'delivered': 'bg-success',
        'cancelled': 'bg-danger'
    };
    
    // Update in orders tab
    const orderRow = document.querySelector(`#orders table tbody tr:nth-child(${orderIndex + 1}) td:nth-child(6) span`);
    if (orderRow) {
        // Remove all status classes and add new one
        orderRow.className = 'badge ' + (statusBadgeClassMap[newStatus] || 'bg-secondary');
        orderRow.textContent = statusCapitalized;
    }
    
    // Update in dashboard (if visible)
    const dashboardOrderRow = document.querySelector(`#dashboard .recent-orders tbody tr:nth-child(${orderIndex + 1}) td:nth-child(6) span`);
    if (dashboardOrderRow) {
        dashboardOrderRow.className = 'badge ' + (statusBadgeClassMap[newStatus] || 'bg-secondary') + ' badge-status';
        dashboardOrderRow.textContent = statusCapitalized;
    }
    
    // Close all modals
    const orderDetailsModal = bootstrap.Modal.getInstance(document.getElementById('orderDetailsModal'));
    const updateStatusModal = bootstrap.Modal.getInstance(document.getElementById('updateOrderStatusModal'));
    
    if (updateStatusModal) updateStatusModal.hide();
    if (orderDetailsModal) orderDetailsModal.hide();
    
    // Show success toast
    showToast(`Order #${orderId} status updated to ${statusCapitalized}`, 'success');
}

function handlePrintInvoice(orderId) {
    // In a real app, this would generate and download an invoice PDF
    console.log(`Printing invoice for order: ${orderId}`);
    
    // For demo purposes, just show a toast
    showToast('Invoice printing initiated', 'info');
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

// Customer Notes Function
function handleAddCustomerNote() {
    const customerId = document.getElementById('customerDetailsId').value;
    const noteText = document.getElementById('customerNoteText').value;
    
    if (!noteText.trim()) {
        showToast('Please enter a note', 'warning');
        return;
    }
    
    // In a real app, this would save the note to the server
    // For demo, we'll just add it to the UI
    
    const notesList = document.getElementById('customerNotesList');
    const noteItem = document.createElement('div');
    noteItem.className = 'customer-note-item border-bottom pb-2 mb-2';
    
    const today = new Date();
    const dateStr = today.toLocaleString();
    
    noteItem.innerHTML = `
        <div class="d-flex justify-content-between">
            <small class="text-muted">${dateStr}</small>
            <small class="text-muted">Admin</small>
        </div>
        <p class="mb-0">${noteText}</p>
    `;
    
    notesList.insertBefore(noteItem, notesList.firstChild);
    
    // Clear input
    document.getElementById('customerNoteText').value = '';
    
    // Show success message
    showToast('Note added successfully');
}

// Customer Functions
function showCustomerDetails(customerId) {
    const customer = customers.find(c => c.id === customerId);
    if (!customer) return;
    
    // Store customer ID for reference
    document.getElementById('customerDetailsId').value = customer.id;
    
    // Populate basic info
    document.getElementById('customerName').textContent = customer.name;
    document.getElementById('customerEmail').textContent = customer.email;
    document.getElementById('customerPhone').textContent = customer.phone;
    
    // Populate stats
    document.getElementById('customerOrderCount').textContent = customer.orders;
    document.getElementById('customerTotalSpent').textContent = `$${customer.totalSpent.toFixed(2)}`;
    document.getElementById('customerLastOrder').textContent = customer.joined; // Using joined date as last order for demo
    
    // Populate recent orders (mock data for demo)
    const recentOrdersContainer = document.getElementById('customerRecentOrders');
    recentOrdersContainer.innerHTML = '';
    
    // Find orders for this customer
    const customerOrders = orders.filter(o => o.customer === customer.name);
    
    if (customerOrders.length > 0) {
        customerOrders.forEach(order => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${order.id}</td>
                <td>${order.date}</td>
                <td>$${order.total.toFixed(2)}</td>
                <td><span class="badge ${getBadgeClassForStatus(order.status)}">${order.status}</span></td>
            `;
            recentOrdersContainer.appendChild(row);
        });
    } else {
        // Demo fallback orders
        const demoOrders = [
            { id: `#ORD-${5400 + customer.id}`, date: '2025-05-01', amount: 799.99, status: 'Delivered' },
            { id: `#ORD-${5300 + customer.id}`, date: '2025-04-15', amount: 1249.98, status: 'Delivered' },
            { id: `#ORD-${5200 + customer.id}`, date: '2025-03-22', amount: 349.99, status: 'Delivered' }
        ];
        
        demoOrders.forEach(order => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${order.id}</td>
                <td>${order.date}</td>
                <td>$${order.amount.toFixed(2)}</td>
                <td><span class="badge ${getBadgeClassForStatus(order.status)}">${order.status}</span></td>
            `;
            recentOrdersContainer.appendChild(row);
        });
    }
    
    // Show modal
    const modal = new bootstrap.Modal(document.getElementById('customerDetailsModal'));
    modal.show();
}

function getBadgeClassForStatus(status) {
    const statusMap = {
        'Delivered': 'bg-success',
        'Shipped': 'bg-info',
        'Processing': 'bg-warning text-dark',
        'Pending': 'bg-secondary',
        'Cancelled': 'bg-danger'
    };
    
    return statusMap[status] || 'bg-secondary';
}

function showEditCustomerModal(customerId) {
    const customer = customers.find(c => c.id === customerId);
    if (!customer) return;
    
    // In a real app, this would populate a form with customer data
    // For demo, we'll just log it
    console.log(`Edit customer with ID: ${customerId}`, customer);
    
    // Show success message
    showToast('Edit customer functionality would open a form to edit customer details');
}

function handleEditCustomer(event) {
    event.preventDefault();
    
    // In a real app, this would process the form data and update the customer
    // For demo, we'll just show a success message
    showToast('Customer details updated successfully!');
    
    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('editCustomerModal'));
    if (modal) modal.hide();
}
