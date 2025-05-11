// Admin Dashboard Charts

// Chart initialization
function initSalesChart() {
    const ctx = document.getElementById('salesChart').getContext('2d');
    
    // Sales data for the last 6 months
    const salesData = {
        labels: ['Dec', 'Jan', 'Feb', 'Mar', 'Apr', 'May'],
        datasets: [
            {
                label: 'Sales Revenue ($)',
                data: [32450, 38250, 42870, 50120, 57680, 63250],
                backgroundColor: 'rgba(89, 171, 110, 0.2)',
                borderColor: 'rgba(89, 171, 110, 1)',
                borderWidth: 2,
                tension: 0.3
            },
            {
                label: 'Number of Orders',
                data: [124, 145, 167, 189, 203, 225],
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 2,
                tension: 0.3,
                yAxisID: 'y1'
            }
        ]
    };
    
    new Chart(ctx, {
        type: 'line',
        data: salesData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Revenue ($)'
                    }
                },
                y1: {
                    beginAtZero: true,
                    position: 'right',
                    grid: {
                        drawOnChartArea: false
                    },
                    title: {
                        display: true,
                        text: 'Number of Orders'
                    }
                }
            }
        }
    });
}

function initCategoryChart() {
    const ctx = document.getElementById('categoryChart').getContext('2d');
    
    // Get sorted categories by product count
    const sortedCategories = [
        { name: "Smartphones", productsCount: 42 },
        { name: "Laptops & Computers", productsCount: 38 },
        { name: "Audio & Headphones", productsCount: 25 },
        { name: "TV & Home Theater", productsCount: 18 },
        { name: "Gaming", productsCount: 27 }
    ];
    
    const categoryData = {
        labels: sortedCategories.map(cat => cat.name),
        datasets: [{
            label: 'Number of Products',
            data: sortedCategories.map(cat => cat.productsCount),
            backgroundColor: [
                'rgba(89, 171, 110, 0.7)',
                'rgba(54, 162, 235, 0.7)',
                'rgba(255, 206, 86, 0.7)',
                'rgba(75, 192, 192, 0.7)',
                'rgba(153, 102, 255, 0.7)'
            ],
            borderWidth: 1
        }]
    };
    
    new Chart(ctx, {
        type: 'doughnut',
        data: categoryData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right'
                }
            }
        }
    });
}

// Initialize charts when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Initialize charts if they exist
    if(document.getElementById('salesChart')) {
        initSalesChart();
    }
    
    if(document.getElementById('categoryChart')) {
        initCategoryChart();
    }
});
